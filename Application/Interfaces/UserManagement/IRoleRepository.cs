using Marketoo.Application.DTOs.Authentication;

namespace Marketoo.Application.Interfaces.UserManagement
{
    public interface IRoleRepository
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<bool> DeleteRoleAsync(string roleId);
        Task<RoleDto> GetRoleByNameAsync(string roleName);
    }
}
