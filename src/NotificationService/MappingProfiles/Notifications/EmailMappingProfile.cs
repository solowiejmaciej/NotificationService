#region

using AutoMapper;
using NotificationService.Entities.NotificationEntities;
using NotificationService.MediatR.Handlers.CreateNew;
using NotificationService.MediatR.Handlers.GetById;
using NotificationService.Models.Dtos;

#endregion

namespace NotificationService.MappingProfiles.Notifications;

public class EmailMappingProfile : Profile
{
    public EmailMappingProfile()
    {
        CreateMap<EmailNotificationDto, EmailNotification>();
        CreateMap<EmailNotification, EmailNotificationDto>();

        CreateMap<CreateNewEmailCommand, EmailNotification>();
        CreateMap<EmailNotification, CreateNewEmailCommand>();

        CreateMap<GetEmailByIdQuerry, EmailNotificationDto>();
        CreateMap<EmailNotificationDto, GetEmailByIdQuerry>();
    }
}