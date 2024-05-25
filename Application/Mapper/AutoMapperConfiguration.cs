using Microsoft.Extensions.DependencyInjection;

namespace Marketoo.Application.Mapper
{
    public static class AutoMapperConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperConfiguration),
                                   typeof(UserMappingProfile),
                                   typeof(RoleMappingProfile),
                                   typeof(CategoryMappingProfile),
                                   typeof(ProductMappingProfile),
                                   typeof(AddProductMappingProfile));
        }
    }
}
