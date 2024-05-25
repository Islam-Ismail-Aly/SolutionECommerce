
namespace Marketoo.ECommerceAPI.Admin.Controllers
{
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    [Route("api/admin/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "AdminAPIv1")]
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


        [HttpPost("AddProduct")]
        [SwaggerOperation(Summary = ProductControllerSwaggerAttributes.AddProductSummary)]
        [SwaggerResponse(200, ProductControllerSwaggerAttributes.AddProductResponse200, typeof(APIResponseResult<ProductDto>))]
        [SwaggerResponse(400, ProductControllerSwaggerAttributes.AddProductResponse400)]
        public async Task<ActionResult<APIResponseResult<AddProductDto>>> AddProduct([FromBody] AddProductDto productDto)
        {
            if (productDto == null)
                return BadRequest(new APIResponseResult<ProductDto>("Invalid product data."));

            try
            {
                var product = _mapper.Map<Product>(productDto);

                await _unitOfWork.Entity.InsertAsync(product);
                await _unitOfWork.SaveAsync();

                var createdProductDto = _mapper.Map<AddProductDto>(product);
                return Ok(new APIResponseResult<AddProductDto>(createdProductDto, "Product added successfully."));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new APIResponseResult<AddProductDto>("An error occurred while saving the product."));
            }
        }



        [HttpPut("UpdateProduct/{id:int}")]
        [SwaggerOperation(Summary = ProductControllerSwaggerAttributes.UpdateProductSummary)]
        [SwaggerResponse(200, ProductControllerSwaggerAttributes.UpdateProductResponse200, typeof(APIResponseResult<ProductDto>))]
        [SwaggerResponse(400, ProductControllerSwaggerAttributes.UpdateProductResponse400)]
        [SwaggerResponse(404, ProductControllerSwaggerAttributes.UpdateProductResponse404)]
        public async Task<ActionResult<APIResponseResult<ProductDto>>> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (productDto == null)
                return BadRequest(new APIResponseResult<ProductDto>("Invalid product data."));

            var productInDb = await _unitOfWork.Entity.GetByIdAsync(id);
            if (productInDb == null)
                return NotFound(new APIResponseResult<ProductDto>("Product not found."));

            await _unitOfWork.Entity.UpdateAsync(productInDb);
            await _unitOfWork.SaveAsync();

            var updatedDto = _mapper.Map<ProductDto>(productInDb);
            return Ok(new APIResponseResult<ProductDto>(updatedDto, "Product updated successfully."));
        }


        [HttpDelete("DeleteProduct/{id:int}")]
        [SwaggerOperation(Summary = ProductControllerSwaggerAttributes.DeleteProductSummary)]
        [SwaggerResponse(200, ProductControllerSwaggerAttributes.DeleteProductResponse200)]
        [SwaggerResponse(404, ProductControllerSwaggerAttributes.DeleteProductResponse404)]
        public async Task<ActionResult<APIResponseResult<ProductDto>>> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Entity.GetByIdAsync(id);
            if (product == null)
                return NotFound(new APIResponseResult<ProductDto>("Product not found."));

            _unitOfWork.Entity.DeleteAsync(product);
            await _unitOfWork.SaveAsync();

            return Ok(new APIResponseResult<ProductDto>(null, "Product deleted successfully."));
        }

    }
}
