using System.Threading;
using System.Threading.Tasks;
using AuthService.Application.Models.Responses;
using AuthService.Application.Services;
using MediatR;

namespace AuthService.Application.MediatR.Command;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
{
    private readonly IJwtManager _jwtManager;

    public RefreshTokenCommandHandler(
        IJwtManager jwtManager
        )
    {
        _jwtManager = jwtManager;
    }
    
    public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
       return await _jwtManager.RefreshTokenAsync(request);
    }
}

public class RefreshTokenCommand : IRequest<TokenResponse>
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}