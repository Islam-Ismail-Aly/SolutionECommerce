namespace Marketoo.ECommerceAPI.Domain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "AuthenticationAPIv1")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountAuthenticationService _authenticationService;

        public AccountController(IAccountAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            var result = await _authenticationService.LoginAsync(loginDto);

            if (!result.IsAuthenticated)
                return BadRequest(new { result.Message, result.IsAuthenticated });

            return Ok(new APIResponseResult<AuthenticationDto>(result, "Login successfully."));
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
        {
            var result = await _authenticationService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return StatusCode(StatusCodes.Status400BadRequest, new APIResponseResult<JsonContent>(ModelState.ToString()));

            return Ok(new { token = result.Token, result.ExpiresOn });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddCustomRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleDto model)
        {
            var result = await _authenticationService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        #region GetRoles old implementation
        //[HttpGet("GetAllRoles")]
        //public async Task<IActionResult> GetRoles()
        //{
        //    var users = await _userManager.Users.AsNoTracking().ToListAsync();
        //    List<RoleDto> roleDtos = new List<RoleDto>();

        //    if (!users.IsNullOrEmpty())
        //    {
        //        foreach (var user in users)
        //        {
        //            var roles = await _userManager.GetRolesAsync(user);

        //            if (roles == null)
        //            {
        //                return NotFound(new APIResponseResult<JsonContent>("No roles found."));
        //            }

        //            foreach (var role in roles)
        //            {
        //                roleDtos.Add(new RoleDto
        //                {
        //                    UserId = user.Id,
        //                    Role = role
        //                });
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, new APIResponseResult<JsonContent>("there something wrong when loading GetAllRoles"));
        //    }

        //    return Ok(new APIResponseResult<List<RoleDto>>(roleDtos, "Roles retrieved successfully."));
        //}
        #endregion

    }
}
