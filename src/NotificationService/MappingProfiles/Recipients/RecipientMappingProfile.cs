using AutoMapper;
using NotificationService.Entities;
using NotificationService.Models;
using NotificationService.Models.Dtos;

namespace NotificationService.MappingProfiles.Recipients
{
    public class RecipientMappingProfile : Profile
    {
        public RecipientMappingProfile()
        {
            CreateMap<UserDto, Recipient>()
                .ForMember(u => u.UserId, c => c.MapFrom(r => r.Id));
            CreateMap<Recipient, UserDto>()
                .ForMember(u => u.Id, c => c.MapFrom(r => r.UserId));
        }
    }
}