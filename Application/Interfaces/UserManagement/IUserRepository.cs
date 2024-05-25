using Marketoo.Application.DTOs.UserManagement;
using Marketoo.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Marketoo.Application.Interfaces.UserManagement
{
    public interface IUserRepository
    {
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
        Task<int> CountAsync();
    }
}
