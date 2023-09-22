using AutoMapper;
using MediatR;
using NotificationService.Entities.NotificationEntities;
using Shared.Exceptions;
using NotificationService.Hangfire.Manager;
using NotificationService.Repositories;
using NotificationService.Services;

namespace NotificationService.MediatR.Handlers.CreateNew
{
    public class CreateNewEmailCommandHandler : IRequestHandler<CreateNewEmailCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IEmailsRepository _repository;
        private readonly INotificationJobManager _jobManager;
        private readonly IRecipientService _recipientService;

        public CreateNewEmailCommandHandler(
            IMapper mapper,
            IEmailsRepository repository,
            INotificationJobManager jobManager,
            IRecipientService recipientService
        )
        {
            _mapper = mapper;
            _repository = repository;
            _jobManager = jobManager;
            _recipientService = recipientService;
        }

        public async Task<int> Handle(CreateNewEmailCommand request, CancellationToken cancellationToken)
        {
            var recipient = await _recipientService.GetRecipientFromUserId(request.RecipiantId);

            if (recipient is null)
            {
                throw new NotFoundException("Recipient not found");
            }
            
            var email = _mapper.Map<EmailNotification>(request);

            email.RecipientId = request.RecipiantId;
            await _repository.AddAsync(email, cancellationToken);
            await _repository.SaveAsync(cancellationToken);
            _jobManager.EnqueueEmailDeliveryDeliveryJob(email, recipient);
            return email.Id;
        }
    }
    
    public record CreateNewEmailCommand : IRequest<int>
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public string RecipiantId { get; set; }
    }
}