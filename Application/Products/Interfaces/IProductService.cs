using Application.Products.DTOs;
using Application.Responses;

namespace Application.Products.Interfaces;

public interface IProductService
{
    Task<Response<List<ProductDto>>> GetAllProductsAsync(int pageNumber, int pageSize);
    Task<Response<ProductDto>> GetProductByIdAsync(int id);
    Task<Response<List<ProductDto>>> GetProductsByCategoryAsync(int categoryId);
    Task<Response<List<ProductDto>>> SearchProductsAsync(string name);
    Task<Response<string>> CreateProductAsync(CreateProductDto product);
    Task<Response<string>> UpdateProductAsync(int id, UpdateProductDto product);
    Task<Response<string>> DeleteProductAsync(int id);
}
