using Application.Categories.DTOs;
using Application.Responses;

namespace Application.Categories.Interfaces;

public interface ICategoryService
{
    Task<Response<List<CategoryDto>>> GetAllCategoriesAsync();
    Task<Response<CategoryDto>> GetCategoryByIdAsync(int id);
    Task<Response<string>> CreateCategoryAsync(CreateCategoryDto category);
    Task<Response<string>> UpdateCategoryAsync(int id, UpdateCategoryDto category);
    Task<Response<string>> DeleteCategoryAsync(int id);
    Task<Response<int>> GetProductsCountAsync(int id);
}
