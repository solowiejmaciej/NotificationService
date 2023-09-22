using AutoMapper;
using NotificationService.Entities.NotificationEntities;
using NotificationService.MediatR.Handlers.CreateNew;
using NotificationService.MediatR.Handlers.GetById;
using NotificationService.Models.Dtos;

namespace NotificationService.MappingProfiles.Notifications;

public class SmsMappingProfile : Profile
{
    public SmsMappingProfile()
    {
        CreateMap<SmsNotificationDto, SmsNotification>();
        CreateMap<SmsNotification, SmsNotificationDto>();

        CreateMap<CreateNewSmsCommand, SmsNotification>();
        CreateMap<SmsNotification, CreateNewSmsCommand>();

        CreateMap<GetSmsByIdQuerry, SmsNotificationDto>();
        CreateMap<SmsNotificationDto, GetSmsByIdQuerry>();
    }
}