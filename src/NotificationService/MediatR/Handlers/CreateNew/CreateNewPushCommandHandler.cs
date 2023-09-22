using AutoMapper;
using MediatR;
using NotificationService.Entities.NotificationEntities;
using Shared.Exceptions;
using NotificationService.Hangfire.Manager;
using NotificationService.Repositories;
using NotificationService.Services;

namespace NotificationService.MediatR.Handlers.CreateNew
{
    public class CreateNewPushCommandHandler : IRequestHandler<CreateNewPushCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IPushRepository _repository;
        private readonly INotificationJobManager _jobManager;
        private readonly IRecipientService _recipientService;

        public CreateNewPushCommandHandler(
            IMapper mapper,
            IPushRepository repository,
            INotificationJobManager jobManager,
            IRecipientService recipientService
        )
        {
            _mapper = mapper;
            _repository = repository;
            _jobManager = jobManager;
            _recipientService = recipientService;
        }

        public async Task<int> Handle(CreateNewPushCommand request, CancellationToken cancellationToken)
        {
            var push = _mapper.Map<PushNotification>(request);

            var recipient = await _recipientService.GetRecipientFromUserId(request.RecipiantId);

            if (recipient is null)
            {
                throw new NotFoundException("Recipient not found");
            }

            if (recipient.DeviceId is null)
            {
                throw new UnprocessableContentException($"User {recipient.UserId} DeviceId not found");
            }

            push.RecipientId = request.RecipiantId;
            await _repository.AddAsync(push, cancellationToken);
            await _repository.SaveAsync(cancellationToken);
            _jobManager.EnqueuePushDeliveryDeliveryJob(push, recipient);
            return push.Id;
        }
    }
    
    public record CreateNewPushCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string RecipiantId { get; set; }
    }
}