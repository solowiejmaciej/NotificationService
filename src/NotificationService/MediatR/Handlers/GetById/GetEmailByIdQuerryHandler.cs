﻿using AutoMapper;
using MediatR;
using Shared.Exceptions;
using NotificationService.Models.Dtos;
using NotificationService.Repositories;
using Shared.UserContext;

namespace NotificationService.MediatR.Handlers.GetById
{
    public class GetEmailByIdQuerryHandler : IRequestHandler<GetEmailByIdQuerry, EmailNotificationDto>
    {
        private readonly IEmailsRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public GetEmailByIdQuerryHandler(
            IEmailsRepository repository,
            IMapper mapper,
            IUserContext userContext
            )
        {
            _repository = repository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<EmailNotificationDto> Handle(GetEmailByIdQuerry request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            var email = await _repository.GetEmailByIdAndUserIdAsync(request.Id, currentUser.Id, cancellationToken);

            if (email == null)
            {
                throw new NotFoundException($"Email with id {request.Id} not found");
            }

            var dto = _mapper.Map<EmailNotificationDto>(email);
            return dto;
        }
    }
    
    public record GetEmailByIdQuerry : IRequest<EmailNotificationDto>
    {
        public int Id { get; set; }
    }
}