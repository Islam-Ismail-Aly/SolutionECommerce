using AutoMapper;
using Marketoo.Application.DTOs.Product;
using Marketoo.Core.Entities;

namespace Marketoo.Application.Mapper
{
    public class AddProductMappingProfile : Profile
    {
        public AddProductMappingProfile()
        {
            CreateMap<Product, AddProductDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryId))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .AfterMap((dto, product) => {
                    product.CategoryId = dto.Category;
                });
        }
    }

}
