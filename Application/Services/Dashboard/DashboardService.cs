using Marketoo.Application.DTOs.Dashboard;
using Marketoo.Application.Interfaces;
using Marketoo.Application.Interfaces.Dashboard;
using Marketoo.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Marketoo.Application.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly ICommonRepository<ApplicationUser> _userRepository;
        private readonly ICommonRepository<Product> _productRepository;
        private readonly ICommonRepository<Category> _categoryRepository;
        private readonly ICommonRepository<IdentityRole> _roleRepository;

        public DashboardService(ICommonRepository<ApplicationUser> userRepository,
                                ICommonRepository<Product> productRepository,
                                ICommonRepository<Category> categoryRepository,
                                ICommonRepository<IdentityRole> roleRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _roleRepository = roleRepository;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var dashboardData = new DashboardDto
            {
                UsersCount = await _userRepository.CountAsync(),
                ProductsCount = await _productRepository.CountAsync(),
                CategoriesCount = await _categoryRepository.CountAsync(),
                RolesCount = await _roleRepository.CountAsync()
            };

            return dashboardData;
        }
    }
}
