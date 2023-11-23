#region

using AuthService.Application.Models.Responses;
using AuthService.Application.Services;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Exceptions;

#endregion

namespace AuthService.Application.MediatR.Query;

public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, TokenResponse>
{
    private readonly IJwtManager _jwtManager;
    private readonly IUsersRepository _repository;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

    public GetTokenQueryHandler(
        IJwtManager jwtManager,
        IUsersRepository dbContext,
        IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _jwtManager = jwtManager;
        _repository = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<TokenResponse> Handle(GetTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null) throw new BadRequestException("Invalid username or password");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed) throw new BadRequestException("Invalid username or password");

        return await _jwtManager.GenerateJwtAsync(user);
    }
}

public record GetTokenQuery : IRequest<TokenResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}