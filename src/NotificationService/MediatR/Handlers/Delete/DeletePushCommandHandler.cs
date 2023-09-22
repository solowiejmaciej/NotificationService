﻿using MediatR;
using Shared.Exceptions;
using NotificationService.Repositories;
using Shared.UserContext;

namespace NotificationService.MediatR.Handlers.Delete
{
    public class DeletePushCommandHandler : IRequestHandler<DeletePushCommand>
    {
        private readonly IPushRepository _repository;
        private readonly IUserContext _userContext;

        public DeletePushCommandHandler(
            IPushRepository repository,
            IUserContext userContext
        )
        {
            _repository = repository;
            _userContext = userContext;
        }

        public async Task Handle(DeletePushCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            var deletedPushId = await _repository.SoftDeleteAsync(request.Id, user.Id, cancellationToken);

            if (deletedPushId == 0)
            {
                throw new NotFoundException($"Push with Id {request.Id} not found");
            }
        }
    }
    
    public record DeletePushCommand : IRequest
    {
        public int Id { get; set; }
    }
}