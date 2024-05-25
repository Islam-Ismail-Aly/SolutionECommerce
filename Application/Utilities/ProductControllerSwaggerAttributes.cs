namespace Marketoo.Application.Utilities
{
    public class ProductControllerSwaggerAttributes
    {
        public const string GetAllProductsSummary = "Retrieves all products, including their categories.";
        public const string GetProductByIdSummary = "Retrieves a single product by ID, including its category.";
        public const string UpdateProductSummary = "Updates an existing product.";
        public const string DeleteProductSummary = "Deletes an existing product.";

        public const string GetAllProductsResponse200 = "A list of products with detailed information.";
        public const string GetAllProductsResponse404 = "If no products are found.";

        public const string GetProductByIdResponse200 = "If the product is found.";
        public const string GetProductByIdResponse404 = "If the product is not found.";

        public const string UpdateProductResponse200 = "If the product is updated successfully.";
        public const string UpdateProductResponse400 = "If the provided product data is invalid.";

        public const string UpdateProductResponse404 = "If the product is not found.";
        public const string DeleteProductResponse200 = "If the product is deleted successfully.";
        public const string DeleteProductResponse404 = "If the product is not found.";

        public const string AddProductSummary = "Adds a new product.";
        public const string AddProductResponse200 = "Product added successfully.";
        public const string AddProductResponse400 = "Invalid product data.";
    }
}
