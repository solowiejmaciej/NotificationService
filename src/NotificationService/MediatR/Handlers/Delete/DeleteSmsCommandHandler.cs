using MediatR;
using Shared.Exceptions;
using NotificationService.Repositories;
using Shared.UserContext;

namespace NotificationService.MediatR.Handlers.Delete
{
    public class DeleteSmsCommandHandler : IRequestHandler<DeleteSmsCommand>
    {
        private readonly ISmsRepository _repository;
        private readonly IUserContext _userContext;

        public DeleteSmsCommandHandler(
            ISmsRepository repository,
            IUserContext userContext
        )
        {
            _repository = repository;
            _userContext = userContext;
        }

        public async Task Handle(DeleteSmsCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            var deletedSmsId = await _repository.SoftDeleteAsync(request.Id, user.Id, cancellationToken);

            if (deletedSmsId == 0)
            {
                throw new NotFoundException($"Sms with Id {request.Id} not found");
            }
        }
    }
    
    public record DeleteSmsCommand : IRequest
    {
        public int Id { get; set; }
    }
}