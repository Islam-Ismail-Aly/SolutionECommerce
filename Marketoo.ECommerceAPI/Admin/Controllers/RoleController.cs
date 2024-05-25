
namespace Marketoo.ECommerceAPI.Admin.Controllers
{
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    [Route("api/admin/[controller]")]
    [ApiExplorerSettings(GroupName = "AdminAPIv1")]
    [Produces("application/json")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }


        [HttpPost("CreateRole")]
        [SwaggerOperation(Summary = RoleControllerSwaggerAttributes.CreateRoleSummary)]
        [SwaggerResponse(201, RoleControllerSwaggerAttributes.CreateRoleResponse201)]
        [SwaggerResponse(400, RoleControllerSwaggerAttributes.CreateRoleResponse400)]
        public async Task<ActionResult<APIResponseResult<RoleDto>>> CreateRole([FromBody] string roleName)
        {
            if (roleName == null)
            {
                return BadRequest(new APIResponseResult<RoleDto>("Invalid role data."));
            }
            // Create the role
            await _roleRepository.CreateRoleAsync(roleName);

            var result = await _roleRepository.GetRoleByNameAsync(roleName);

            // Return response
            return Ok(new APIResponseResult<RoleDto>(result, "Role created successfully."));
        }


        [HttpGet(Name = "GetRoleByName")]
        [SwaggerOperation(Summary = RoleControllerSwaggerAttributes.GetRoleByNameSummary)]
        [SwaggerResponse(200, RoleControllerSwaggerAttributes.GetRoleByNameResponse200, typeof(RoleDto))]
        [SwaggerResponse(404, RoleControllerSwaggerAttributes.GetRoleByNameResponse404)]
        public async Task<ActionResult<RoleDto>> GetRoleByName(string roleName)
        {
            var role = await _roleRepository.GetRoleByNameAsync(roleName);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }


        [HttpGet("GetAllRoles")]
        [SwaggerOperation(Summary = RoleControllerSwaggerAttributes.GetAllRolesSummary)]
        [SwaggerResponse(200, RoleControllerSwaggerAttributes.GetAllRolesResponse200, typeof(IEnumerable<RoleDto>))]
        public async Task<ActionResult<APIResponseResult<IEnumerable<RoleDto>>>> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return Ok(new APIResponseResult<IEnumerable<RoleDto>>(roles, "Roles retrieved successfully."));
        }


        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = RoleControllerSwaggerAttributes.DeleteRoleSummary)]
        [SwaggerResponse(200, RoleControllerSwaggerAttributes.DeleteRoleResponse200)]
        [SwaggerResponse(400, RoleControllerSwaggerAttributes.DeleteRoleResponse400)]
        [SwaggerResponse(404, RoleControllerSwaggerAttributes.DeleteRoleResponse404)]
        [SwaggerResponse(500, RoleControllerSwaggerAttributes.DeleteRoleResponse500)]
        public async Task<ActionResult<APIResponseResult<object>>> DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new APIResponseResult<object>("Invalid role ID provided."));
            }

            try
            {
                var success = await _roleRepository.DeleteRoleAsync(id);
                if (!success)
                {
                    return NotFound(new APIResponseResult<object>("Role not found."));
                }

                return Ok(new APIResponseResult<object>(null, "Role deleted successfully."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new APIResponseResult<object>(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponseResult<object>("An error occurred while deleting the role."));
            }
        }

    }
}
