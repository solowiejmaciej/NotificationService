#region

using AuthService.Application.Dtos;
using AuthService.Domain.Entities;
using AutoMapper;

#endregion

namespace AuthService.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserDto, ApplicationUser>();
        CreateMap<ApplicationUser, UserDto>();
    }
}