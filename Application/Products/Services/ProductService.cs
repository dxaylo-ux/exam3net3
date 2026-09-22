using Application.DateTimes.Interfaces;
using Application.Products.DTOs;
using Application.Products.Interfaces;
using Application.Responses;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Products.Services;

public class ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IDateTimeService dateTimeService, ILogger<ProductService> logger) : IProductService
{
    private readonly IProductRepository _repository = productRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IDateTimeService _dateTime = dateTimeService;
    private readonly ILogger<ProductService> _logger = logger;

    public async Task<Response<List<ProductDto>>> GetAllProductsAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1)
            return new Response<List<ProductDto>>(400, "Page number and page size must be greater than 0");

        try
        {
            var offset = (pageNumber - 1) * pageSize;
            var res = await _repository.GetAllProductsAsync(pageSize, offset);
            var data = res.Select(x => new ProductDto
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Quantity = x.Quantity,
                CreatedAt = x.CreatedAt
            }).ToList();
            return new Response<List<ProductDto>>(200, "Products retrieved successfully", data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting products");
            return new Response<List<ProductDto>>(500, "Internal server error");
        }
    }

    public async Task<Response<ProductDto>> GetProductByIdAsync(int id)
    {
        try
        {
            var x = await _repository.GetProductByIdAsync(id);
            if (x==null)
                return new Response<ProductDto>(404, "Product not found");

            var data = new ProductDto
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Quantity = x.Quantity,
                CreatedAt = x.CreatedAt
            };
            return new Response<ProductDto>(200, "Product retrieved successfully", data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting product by id");
            return new Response<ProductDto>(500, "Internal server error");
        }
    }

    public async Task<Response<List<ProductDto>>> GetProductsByCategoryAsync(int categoryId)
    {
        try
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category==null)
                return new Response<List<ProductDto>>(404, "Category not found");

            var res = await _repository.GetProductsByCategoryAsync(categoryId);
            var data = res.Select(x => new ProductDto
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Quantity = x.Quantity,
                CreatedAt = x.CreatedAt
            }).ToList();
            return new Response<List<ProductDto>>(200, "Products retrieved successfully", data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting products by category");
            return new Response<List<ProductDto>>(500, "Internal server error");
        }
    }

    public async Task<Response<List<ProductDto>>> SearchProductsAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new Response<List<ProductDto>>(400, "Search name is required");

        try
        {
            var res = await _repository.SearchProductsAsync(name);
            var data = res.Select(x => new ProductDto
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Quantity = x.Quantity,
                CreatedAt = x.CreatedAt
            }).ToList();
            return new Response<List<ProductDto>>(200, "Products retrieved successfully", data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while searching products");
            return new Response<List<ProductDto>>(500, "Internal server error");
        }
    }

    public async Task<Response<string>> CreateProductAsync(CreateProductDto product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            return new Response<string>(400, "Product name is required");
        if (product.Price <= 0)
            return new Response<string>(400, "Product price must be greater than 0");
        if (product.Quantity < 0)
            return new Response<string>(400, "Product quantity cannot be less than 0");

        try
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
            if (category==null)
                return new Response<string>(400, "Category does not exist");

            var prod = new Product()
            {
                CategoryId = product.CategoryId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                CreatedAt = _dateTime.GetCurrentDateTime()
            };
            await _repository.CreateProductAsync(prod);
            _logger.LogInformation("Product {Name} created", prod.Name);
            return new Response<string>(200, "Product created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating product");
            return new Response<string>(500, "Internal server error");
        }
    }

    public async Task<Response<string>> UpdateProductAsync(int id, UpdateProductDto product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            return new Response<string>(400, "Product name is required");
        if (product.Price <= 0)
            return new Response<string>(400, "Product price must be greater than 0");
        if (product.Quantity < 0)
            return new Response<string>(400, "Product quantity cannot be less than 0");

        try
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
            if (category==null)
                return new Response<string>(400, "Category does not exist");

            var prod = new Product()
            {
                Id = id,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity
            };
            var res = await _repository.UpdateProductAsync(prod);
            if (res==false)
                return new Response<string>(404, "Product not found");

            _logger.LogInformation("Product {Id} updated", id);
            return new Response<string>(200, "Product updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating product");
            return new Response<string>(500, "Internal server error");
        }
    }

    public async Task<Response<string>> DeleteProductAsync(int id)
    {
        try
        {
            var res = await _repository.DeleteProductAsync(id);
            if (res==false)
                return new Response<string>(404, "Product not found");

            _logger.LogInformation("Product {Id} deleted", id);
            return new Response<string>(200, "Product deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting product");
            return new Response<string>(500, "Internal server error");
        }
    }
}
