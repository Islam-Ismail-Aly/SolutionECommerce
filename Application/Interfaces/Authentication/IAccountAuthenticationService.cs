using Marketoo.Application.DTOs.Authentication;

namespace Marketoo.Application.Interfaces.Authentication
{
    public interface IAccountAuthenticationService
    {
        Task<AuthenticationDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthenticationDto> LoginAsync(LoginDto loginDto);
        Task<string> AddRoleAsync(RoleDto roleDto);
    }
}
