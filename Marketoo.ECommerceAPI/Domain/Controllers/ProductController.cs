using Marketoo.Application.Pagination;

namespace Marketoo.ECommerceAPI.Domain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "ProductAPIv1")]
    [SwaggerTag("Product Management")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork<Product> _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork<Product> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("GetAllProducts")]
        [SwaggerOperation(Summary = ProductControllerSwaggerAttributes.GetAllProductsSummary)]
        [SwaggerResponse(200, ProductControllerSwaggerAttributes.GetAllProductsResponse200, typeof(APIResponseResult<IEnumerable<ProductDto>>))]
        [SwaggerResponse(404, ProductControllerSwaggerAttributes.GetAllProductsResponse404)]
        public async Task<ActionResult<APIResponseResult<IEnumerable<ProductDto>>>> GetAllProducts()
        {
            var products = _unitOfWork.Entity.GetAllIncluding(p => p.Category);
            if (products == null || !products.Any())
                return NotFound(new APIResponseResult<IEnumerable<ProductDto>>("No products found."));

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(new APIResponseResult<IEnumerable<ProductDto>>(productDtos, "Products retrieved successfully."));
        }


        [HttpGet("GetProductById/{id:int}")]
        [SwaggerOperation(Summary = ProductControllerSwaggerAttributes.GetProductByIdSummary)]
        [SwaggerResponse(200, ProductControllerSwaggerAttributes.GetProductByIdResponse200, typeof(APIResponseResult<ProductDto>))]
        [SwaggerResponse(404, ProductControllerSwaggerAttributes.GetProductByIdResponse404)]
        public async Task<ActionResult<APIResponseResult<ProductDto>>> GetProductById(int id)
        {
            var product = _unitOfWork.Entity.GetAllIncluding(p => p.Category).FirstOrDefault(p => p.Id == id);  // Assuming such a method exists
            if (product == null)
                return NotFound(new APIResponseResult<ProductDto>("Product not found."));

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(new APIResponseResult<ProductDto>(productDto, "Product retrieved successfully."));
        }


        [HttpGet("GetPaginatedProducts")]
        [SwaggerOperation(Summary = "Retrieve products with pagination")]
        [SwaggerResponse(200, "Products retrieved successfully.", typeof(APIResponseResult<PaginatedList<ProductDto>>))]
        [SwaggerResponse(404, "No products found.")]
        public async Task<ActionResult<APIResponseResult<PaginatedList<ProductDto>>>> GetPaginatedProducts([FromQuery] PaginationParams paginationParams)
       {
            var query = _unitOfWork.Entity.GetAllIncluding(p => p.Category).AsQueryable();

            var paginatedProducts = await PaginatedList<Product>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
            if (paginatedProducts == null || !paginatedProducts.Any())
                return NotFound(new APIResponseResult<PaginatedList<ProductDto>>("No products found."));

            var productDtos = _mapper.Map< IEnumerable<ProductDto>>(paginatedProducts);
            
            return Ok(new APIResponseResult<IEnumerable<ProductDto>>(productDtos, "Products retrieved successfully.", paginatedProducts.TotalPages));
        }
    }
}
