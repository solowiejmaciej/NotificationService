using System.Threading;
using System.Threading.Tasks;
using AuthService.Application.MediatR.Query;
using AuthService.Application.Models.Responses;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Events;

namespace AuthService.Application.MediatR.Command;

public class CreateNewUserCommandHandler : IRequestHandler<CreateNewUserCommand, TokenResponse>
    {
        private readonly IUsersRepository _userRepository;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMediator _mediator;

        public CreateNewUserCommandHandler(
            IUsersRepository userRepository,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IMediator mediator, IEventPublisher eventPublisher)
        
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mediator = mediator;
            _eventPublisher = eventPublisher;
        }

        public async Task<TokenResponse> Handle(CreateNewUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                NormalizedUserName = request.Email.ToUpper(),
                DeviceId = request.DeviceId,
                PhoneNumber = request.PhoneNumber,
                Firstname = request.Firstname,
                Surname = request.Surname,
            };

            var hashedPass = _passwordHasher.HashPassword(newUser, request.Password);
            newUser.PasswordHash = hashedPass;

            await _userRepository.AddAsyncWithDefaultRole(newUser, cancellationToken);
            
            await _eventPublisher.PublishUserCreatedEventAsync(newUser, cancellationToken);


            var query = new GetTokenQuery()
            {
                Email = request.Email,
                Password = request.Password,
            };

            var response = await _mediator.Send(query, cancellationToken);

            return response;
        }
    }
public record CreateNewUserCommand : IRequest<TokenResponse>
{
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? DeviceId { get; set; }
    public string Firstname { get; set; }
    public string Surname { get; set; }
}