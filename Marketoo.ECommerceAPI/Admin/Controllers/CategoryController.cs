
namespace Marketoo.ECommerceAPI.Admin.Controllers
{
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    [Route("api/admin/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "AdminAPIv1")]
    [SwaggerTag("Category Management")]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork<Category> _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork<Category> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAllCategories")]
        [SwaggerOperation(Summary = "Get all categories")]
        [SwaggerResponse(200, "Returns all categories", typeof(APIResponseResult<IEnumerable<CategoryDto>>))]
        [SwaggerResponse(404, "No categories found")]
        public async Task<ActionResult<APIResponseResult<IEnumerable<CategoryDto>>>> GetAllCategories()
        {
            var categories = await _unitOfWork.Entity.GetAllAsync();
            if (categories == null || !categories.Any())
                return NotFound(new APIResponseResult<IEnumerable<CategoryDto>>("No categories found."));

            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(new APIResponseResult<IEnumerable<CategoryDto>>(categoryDtos, "Categories retrieved successfully."));
        }

        [HttpGet("GetCategoryById/{id:int}")]
        [SwaggerOperation(Summary = "Get a category by ID")]
        [SwaggerResponse(200, "Returns the category", typeof(APIResponseResult<CategoryDto>))]
        [SwaggerResponse(404, "Category not found")]
        public async Task<ActionResult<APIResponseResult<CategoryDto>>> GetCategoryById(int id)
        {
            var category = await _unitOfWork.Entity.GetByIdAsync(id);
            if (category == null)
                return NotFound(new APIResponseResult<CategoryDto>("Category not found."));

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(new APIResponseResult<CategoryDto>(categoryDto, "Category retrieved successfully."));
        }

        [HttpPut("UpdateCategory/{id:int}")]
        [SwaggerOperation(Summary = "Update a category")]
        [SwaggerResponse(200, "Category updated successfully", typeof(APIResponseResult<CategoryDto>))]
        [SwaggerResponse(400, "Invalid category data")]
        [SwaggerResponse(404, "Category not found")]
        public async Task<ActionResult<APIResponseResult<CategoryDto>>> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
                return BadRequest(new APIResponseResult<CategoryDto>("Invalid category data."));

            var categoryInDb = await _unitOfWork.Entity.GetByIdAsync(id);
            if (categoryInDb == null)
                return NotFound(new APIResponseResult<CategoryDto>("Category not found."));

            await _unitOfWork.Entity.UpdateAsync(categoryInDb);
            await _unitOfWork.SaveAsync();

            var updatedDto = _mapper.Map<CategoryDto>(categoryInDb);
            return Ok(new APIResponseResult<CategoryDto>(updatedDto, "Category updated successfully."));
        }

        [HttpDelete("DeleteCategory/{id:int}")]
        [SwaggerOperation(Summary = "Delete a category")]
        [SwaggerResponse(200, "Category deleted successfully")]
        [SwaggerResponse(404, "Category not found")]
        public async Task<ActionResult<APIResponseResult<CategoryDto>>> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Entity.GetByIdAsync(id);
            if (category == null)
                return NotFound(new APIResponseResult<CategoryDto>("Category not found."));

            _unitOfWork.Entity.DeleteAsync(category);
            await _unitOfWork.SaveAsync();

            return Ok(new APIResponseResult<CategoryDto>(null, "Category deleted successfully."));
        }
    }

}
