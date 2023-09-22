using AuthService.Application.Dtos;
using AuthService.Domain.Entities;
using AutoMapper;

namespace AuthService.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDto, ApplicationUser>();
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}