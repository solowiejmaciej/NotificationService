#region

using AutoMapper;
using MediatR;
using NotificationService.Entities.NotificationEntities;
using NotificationService.Hangfire.Manager;
using NotificationService.Repositories;
using NotificationService.Services;
using Shared.Exceptions;

#endregion

namespace NotificationService.MediatR.Handlers.CreateNew;

public class CreateNewSmsCommandHandler : IRequestHandler<CreateNewSmsCommand, int>
{
    private readonly IMapper _mapper;
    private readonly ISmsRepository _repository;
    private readonly INotificationJobManager _jobManager;
    private readonly IRecipientService _recipientService;

    public CreateNewSmsCommandHandler(
        IMapper mapper,
        ISmsRepository repository,
        INotificationJobManager jobManager,
        IRecipientService recipientService
    )
    {
        _mapper = mapper;
        _repository = repository;
        _jobManager = jobManager;
        _recipientService = recipientService;
    }

    public async Task<int> Handle(CreateNewSmsCommand request, CancellationToken cancellationToken)
    {
        var sms = _mapper.Map<SmsNotification>(request);

        var recipient = await _recipientService.GetRecipientFromUserId(request.RecipiantId);

        if (recipient is null) throw new NotFoundException("Recipient not found");

        if (recipient.PhoneNumber is null)
            throw new UnprocessableContentException($"User {recipient.UserId} phone number not found");

        sms.RecipientId = request.RecipiantId;
        await _repository.AddAsync(sms, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _jobManager.EnqueueSmsDeliveryDeliveryJob(sms, recipient);
        return sms.Id;
    }
}

public record CreateNewSmsCommand : IRequest<int>
{
    public string Content { get; set; }
    public string RecipiantId { get; set; }
}