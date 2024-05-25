
namespace Marketoo.ECommerceAPI.Admin.Controllers
{
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    [Route("api/admin/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "AdminAPIv1")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("GetDashboardData")]
        public async Task<ActionResult<DashboardDto>> GetDashboardData()
        {
            var result = await _service.GetDashboardDataAsync();
            return Ok(result);
        }
    }
}
