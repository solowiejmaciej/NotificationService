#region

using MediatR;

#endregion

namespace AuthService.Application.MediatR.Command;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    public Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public record DeleteUserCommand : IRequest
{
    public string Id { get; set; }
}