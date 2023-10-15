#region

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AuthService.Application.MediatR.Command;
using AuthService.Application.Models.AppSettings;
using AuthService.Application.Models.Responses;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Exceptions;

#endregion

namespace AuthService.Application.Services;

public interface IJwtManager
{
    Task<TokenResponse> GenerateJwtAsync(ApplicationUser dbUser);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenCommand command);
}

public class JwtManager : IJwtManager
{
    private readonly IUsersRepository _usersRepository;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly IOptions<JwtSettings> _jwtAppSettings;
    private readonly TokenValidationParameters _refreshTokenValidationParameters;

    public JwtManager(
        IUsersRepository usersRepository,
        IRefreshTokensRepository refreshTokensRepository,
        IOptions<JwtSettings> config,
        TokenValidationParameters refreshTokenValidationParameters
    )
    {
        _usersRepository = usersRepository;
        _refreshTokensRepository = refreshTokensRepository;
        _jwtAppSettings = config;
        _refreshTokenValidationParameters = refreshTokenValidationParameters;
    }

    public async Task<TokenResponse> GenerateJwtAsync(ApplicationUser dbUser)
    {
        var expires = DateTime.Now.AddMinutes(_jwtAppSettings.Value.ExpireMinutes);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, dbUser.Id),
            new(ClaimTypes.Name, dbUser.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var rsaPrivateKey = _jwtAppSettings.Value.PrivateKey;
        using var rsa = RSA.Create();
        rsa.ImportFromPem(rsaPrivateKey);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
        };

        var jwt = new JwtSecurityToken(
            audience: _jwtAppSettings.Value.Issuer,
            issuer: _jwtAppSettings.Value.Issuer,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials
        );

        var refreshToken = new RefreshToken
        {
            JwtId = jwt.Id,
            UserId = dbUser.Id,
            CreationDate = DateTime.Now,
            ExpiryDate = DateTime.Now.AddMonths(6),
            Token = Guid.NewGuid().ToString()
        };

        await _refreshTokensRepository.AddAsync(refreshToken);
        await _refreshTokensRepository.SaveAsync();

        var response = new TokenResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            StatusCode = 200,
            IssuedDate = DateTime.Now,
            ExpiresAt = expires,
            UserId = dbUser.Id,
            RefreshToken = refreshToken.Token
        };

        return response;
    }

    private ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal =
                tokenHandler.ValidateToken(token, _refreshTokenValidationParameters, out var validatedToken);
            if (!IsJwtWithValidSecurityAlgorithm(validatedToken)) return null;

            return principal;
        }
        catch
        {
            return null;
        }
    }

    private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return validatedToken is JwtSecurityToken jwtSecurityToken &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha256,
                   StringComparison.InvariantCultureIgnoreCase);
    }

    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenCommand command)
    {
        var validatedToken = GetPrincipalFromToken(command.Token);

        if (validatedToken is null) throw new BadRequestException("Invalid Token");

        var exipryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateTime = new DateTime(1970, 1, 1, 0, 0, 0)
            .AddSeconds(exipryDateUnix);

        if (expiryDateTime > DateTime.Now) throw new BadRequestException("Token hasn't expired yet");

        var storedRefreshToken = await _refreshTokensRepository.GetByValueAsync(command.RefreshToken);

        if (storedRefreshToken is null) throw new BadRequestException("Token doesn't exists");

        if (DateTime.Now > storedRefreshToken.ExpiryDate) throw new BadRequestException("Token has expired");

        if (storedRefreshToken.Invalidated) throw new BadRequestException("Token has been invalidated");

        if (storedRefreshToken.Used) throw new BadRequestException("Token has been used");

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        if (storedRefreshToken.JwtId != jti)
            throw new BadRequestException("This refresh token does not match this JWT");

        storedRefreshToken.Used = true;
        await _refreshTokensRepository.SetUsedAsync(storedRefreshToken);
        await _refreshTokensRepository.SaveAsync();

        var userId = validatedToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

        var user = await _usersRepository.GetByIdAsync(userId);

        return await GenerateJwtAsync(user);
    }
}