using AutoMapper;
using Marketoo.Application.DTOs.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Marketoo.Application.Mapper
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<IdentityRole, RoleDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Name));
        }
    }
}
