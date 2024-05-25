using AutoMapper;
using Marketoo.Application.DTOs.UserManagement;
using Marketoo.Core.Entities;

namespace Marketoo.Application.Mapper
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            //.ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
