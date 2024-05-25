﻿using Marketoo.Application.DTOs.Authentication;
using Marketoo.Application.Interfaces.Authentication;
using Marketoo.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Marketoo.Application.Services.Authentication
{
    public class AccountAuthenticationService : IAccountAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtService _jwtService;

        public AccountAuthenticationService(UserManager<ApplicationUser> userManager,
                            IOptions<JwtService> jwt,
                            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwt.Value;
        }

        public async Task<AuthenticationDto> LoginAsync(LoginDto loginDto)
        {
            var authenModel = new AuthenticationDto();

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                authenModel.Message = "Invalid Email or Password!";
                return authenModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            authenModel.IsAuthenticated = true;
            authenModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenModel.Email = user.Email;
            authenModel.UserName = user.UserName;
            authenModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authenModel.Roles = roles.ToList();

            return authenModel;
        }

        public async Task<string> AddRoleAsync(RoleDto roleModel)
        {
            var userId = await _userManager.FindByIdAsync(roleModel.UserId);

            if (userId == null || !await _roleManager.RoleExistsAsync(roleModel.Role))
                return "Invalid UserId or Role!";

            var roles = await _userManager.GetRolesAsync(userId);

            if (roles.Contains(roleModel.Role))
                return "User take to this Role!";

            var result = await _userManager.AddToRoleAsync(userId, roleModel.Role);

            return result.Succeeded ? string.Empty : "Error in Adding Role";
        }

        public async Task<AuthenticationDto> RegisterAsync(RegisterDto registerModel)
        {
            if (await _userManager.FindByEmailAsync(registerModel.Email) != null)
            {
                return new AuthenticationDto { Message = "Email is already registered" };
            }

            if (await _userManager.FindByEmailAsync(registerModel.UserName) != null)
            {
                return new AuthenticationDto { Message = "Username is already registered" };
            }

            var user = new ApplicationUser
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Address = registerModel.Address
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }

                return new AuthenticationDto { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthenticationDto
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName,
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Email, user.Email),
                 new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtService.Key));
            var securityCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(_jwtService.DurationInDays),
                signingCredentials: securityCredentials);

            return jwtSecurityToken;
        }
    }
}
