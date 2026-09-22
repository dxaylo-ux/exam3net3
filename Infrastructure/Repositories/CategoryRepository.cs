using Dapper;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class CategoryRepository(DapperContext dapperContext) : ICategoryRepository
{
    private readonly DapperContext _context = dapperContext;

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        using var conn = _context.CreateConnection();
        var query = "select id, name, description, created_at as createdat from categories";
        var res = await conn.QueryAsync<Category>(query);
        return res.ToList();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        using var conn = _context.CreateConnection();
        var query = "select id, name, description, created_at as createdat from categories where id=@id";
        return await conn.QueryFirstOrDefaultAsync<Category>(query, new{id});
    }

    public async Task<bool> CreateCategoryAsync(Category category)
    {
        using var conn = _context.CreateConnection();
        var query = "insert into categories(name, description, created_at) values(@name, @description, @createdat)";
        var res = await conn.ExecuteAsync(query, category);
        return res>0;
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        using var conn = _context.CreateConnection();
        var query = "update categories set name=@name, description=@description where id=@id";
        var res = await conn.ExecuteAsync(query, category);
        return res>0;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        using var conn = _context.CreateConnection();
        var query = "delete from categories where id=@id";
        var res = await conn.ExecuteAsync(query, new{id});
        return res>0;
    }

    public async Task<int> GetProductsCountAsync(int id)
    {
        using var conn = _context.CreateConnection();
        var query = "select count(*) from products where category_id=@id";
        return await conn.QueryFirstOrDefaultAsync<int>(query, new{id});
    }
}
