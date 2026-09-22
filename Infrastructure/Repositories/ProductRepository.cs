using Dapper;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ProductRepository(DapperContext dapperContext) : IProductRepository
{
    private readonly DapperContext _context = dapperContext;

    public async Task<List<Product>> GetAllProductsAsync(int pageSize, int offset)
    {
        using var conn = _context.CreateConnection();
        var query = "select id, category_id as categoryid, name, description, price, quantity, created_at as createdat from products order by id limit @pagesize offset @offset";
        var res = await conn.QueryAsync<Product>(query, new{pageSize, offset});
        return res.ToList();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        using var conn = _context.CreateConnection();
        var query = "select id, category_id as categoryid, name, description, price, quantity, created_at as createdat from products where id=@id";
        return await conn.QueryFirstOrDefaultAsync<Product>(query, new{id});
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        using var conn = _context.CreateConnection();
        var query = "select id, category_id as categoryid, name, description, price, quantity, created_at as createdat from products where category_id=@categoryid";
        var res = await conn.QueryAsync<Product>(query, new{categoryId});
        return res.ToList();
    }

    public async Task<List<Product>> SearchProductsAsync(string name)
    {
        using var conn = _context.CreateConnection();
        var query = "select id, category_id as categoryid, name, description, price, quantity, created_at as createdat from products where name ilike @name";
        var res = await conn.QueryAsync<Product>(query, new{name = $"%{name}%"});
        return res.ToList();
    }

    public async Task<bool> CreateProductAsync(Product product)
    {
        using var conn = _context.CreateConnection();
        var query = "insert into products(category_id, name, description, price, quantity, created_at) values(@categoryid, @name, @description, @price, @quantity, @createdat)";
        var res = await conn.ExecuteAsync(query, product);
        return res>0;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        using var conn = _context.CreateConnection();
        var query = "update products set category_id=@categoryid, name=@name, description=@description, price=@price, quantity=@quantity where id=@id";
        var res = await conn.ExecuteAsync(query, product);
        return res>0;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        using var conn = _context.CreateConnection();
        var query = "delete from products where id=@id";
        var res = await conn.ExecuteAsync(query, new{id});
        return res>0;
    }
}
