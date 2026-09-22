using Application.Categories.DTOs;
using Application.Categories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _service = categoryService;

    [HttpGet]
    public async Task<IActionResult> GetAllCategoriesAsync()
    {
        var res = await _service.GetAllCategoriesAsync();
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryByIdAsync(int id)
    {
        var res = await _service.GetCategoryByIdAsync(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategoryAsync(CreateCategoryDto category)
    {
        var res = await _service.CreateCategoryAsync(category);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategoryAsync(int id, UpdateCategoryDto category)
    {
        var res = await _service.UpdateCategoryAsync(id, category);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoryAsync(int id)
    {
        var res = await _service.DeleteCategoryAsync(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}/products-count")]
    public async Task<IActionResult> GetProductsCountAsync(int id)
    {
        var res = await _service.GetProductsCountAsync(id);
        return StatusCode(res.StatusCode, res);
    }
}
