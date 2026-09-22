using Domain.Models;

namespace Domain.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync(int pageSize, int offset);
    Task<Product?> GetProductByIdAsync(int id);
    Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
    Task<List<Product>> SearchProductsAsync(string name);
    Task<bool> CreateProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int id);
}
