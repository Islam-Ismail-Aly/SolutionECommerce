using AutoMapper;
using Marketoo.Application.DTOs.Authentication;
using Marketoo.Application.Interfaces.UserManagement;
using Marketoo.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketoo.Application.Repositories
{
    public class RoleRepository : IRoleRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleRepository(ApplicationDbContext context, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            try
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                // Log the exception
                return false;
            }
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                throw new ArgumentException("Role ID cannot be null or empty.", nameof(roleId));
            }

            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
            {
                return false;  // Role not found, indicate failure without throwing an error
            }

            try
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return true;  // Role deleted successfully
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _context.Roles.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return null;
            }
            return _mapper.Map<IdentityRole, RoleDto>(role);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
