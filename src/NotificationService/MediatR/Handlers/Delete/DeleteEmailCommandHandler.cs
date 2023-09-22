﻿using MediatR;
using Shared.Exceptions;
using NotificationService.Repositories;
using Shared.UserContext;

namespace NotificationService.MediatR.Handlers.Delete
{
    public class DeleteEmailCommandHandler : IRequestHandler<DeleteEmailCommand>
    {
        private readonly IEmailsRepository _repository;
        private readonly IUserContext _userContext;

        public DeleteEmailCommandHandler(
            IEmailsRepository repository,
            IUserContext userContext
        )
        {
            _repository = repository;
            _userContext = userContext;
        }

        public async Task Handle(DeleteEmailCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            var deletedEmailId = await _repository.SoftDeleteAsync(request.Id, user.Id, cancellationToken);

            if (deletedEmailId == 0)
            {
                throw new NotFoundException($"Email with Id {request.Id} not found");
            }
        }
    }
    
    public record DeleteEmailCommand : IRequest
    {
        public int Id { get; set; }
    }
}