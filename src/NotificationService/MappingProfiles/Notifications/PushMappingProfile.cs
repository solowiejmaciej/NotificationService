using AutoMapper;
using NotificationService.Entities.NotificationEntities;
using NotificationService.MediatR.Handlers.CreateNew;
using NotificationService.MediatR.Handlers.GetById;
using NotificationService.Models.Dtos;

namespace NotificationService.MappingProfiles.Notifications;

public class PushMappingProfile : Profile
{
    public PushMappingProfile()
    {
        CreateMap<PushNotification, PushNotificationDto>();
        CreateMap<PushNotificationDto, PushNotificationDto>();

        CreateMap<CreateNewPushCommand, PushNotification>();
        CreateMap<PushNotification, CreateNewPushCommand>();

        CreateMap<GetPushByIdQuerry, PushNotificationDto>();
        CreateMap<PushNotificationDto, GetPushByIdQuerry>();
    }
}