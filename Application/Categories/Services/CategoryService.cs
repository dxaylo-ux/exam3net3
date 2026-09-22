using Application.Categories.DTOs;
using Application.Categories.Interfaces;
using Application.DateTimes.Interfaces;
using Application.Responses;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Categories.Services;

public class CategoryService(ICategoryRepository categoryRepository, IDateTimeService dateTimeService, ILogger<CategoryService> logger) : ICategoryService
{
    private readonly ICategoryRepository _repository = categoryRepository;
    private readonly IDateTimeService _dateTime = dateTimeService;
    private readonly ILogger<CategoryService> _logger = logger;

    public async Task<Response<List<CategoryDto>>> GetAllCategoriesAsync()
    {
        try
        {
            var res = await _repository.GetAllCategoriesAsync();
            var data = res.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedAt = x.CreatedAt
            }).ToList();
            return new Response<List<CategoryDto>>(200, "Categories retrieved successfully", data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting categories");
            return new Response<List<CategoryDto>>(500, "Internal server error");
        }
    }

    public async Task<Response<CategoryDto>> GetCategoryByIdAsync(int id)
    {
        try
        {
            var x = await _repository.GetCategoryByIdAsync(id);
            if (x==null)
                return new Response<CategoryDto>(404, "Category not found");

            var data = new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedAt = x.CreatedAt
            };
            return new Response<CategoryDto>(200, "Category retrieved successfully", data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting category by id");
            return new Response<CategoryDto>(500, "Internal server error");
        }
    }

    public async Task<Response<string>> CreateCategoryAsync(CreateCategoryDto category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            return new Response<string>(400, "Category name is required");

        try
        {
            var cat = new Category()
            {
                Name = category.Name,
                Description = category.Description,
                CreatedAt = _dateTime.GetCurrentDateTime()
            };
            await _repository.CreateCategoryAsync(cat);
            _logger.LogInformation("Category {Name} created", cat.Name);
            return new Response<string>(200, "Category created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating category");
            return new Response<string>(500, "Internal server error");
        }
    }

    public async Task<Response<string>> UpdateCategoryAsync(int id, UpdateCategoryDto category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            return new Response<string>(400, "Category name is required");

        try
        {
            var cat = new Category()
            {
                Id = id,
                Name = category.Name,
                Description = category.Description
            };
            var res = await _repository.UpdateCategoryAsync(cat);
            if (res==false)
                return new Response<string>(404, "Category not found");

            _logger.LogInformation("Category {Id} updated", id);
            return new Response<string>(200, "Category updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating category");
            return new Response<string>(500, "Internal server error");
        }
    }

    public async Task<Response<string>> DeleteCategoryAsync(int id)
    {
        try
        {
            var res = await _repository.DeleteCategoryAsync(id);
            if (res==false)
                return new Response<string>(404, "Category not found");

            _logger.LogInformation("Category {Id} deleted", id);
            return new Response<string>(200, "Category deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting category");
            return new Response<string>(500, "Internal server error");
        }
    }

    public async Task<Response<int>> GetProductsCountAsync(int id)
    {
        try
        {
            var category = await _repository.GetCategoryByIdAsync(id);
            if (category==null)
                return new Response<int>(404, "Category not found");

            var res = await _repository.GetProductsCountAsync(id);
            return new Response<int>(200, "Products count retrieved successfully", res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting products count");
            return new Response<int>(500, "Internal server error");
        }
    }
}
