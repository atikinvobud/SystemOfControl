using BackEnd.Entities;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly Context context;
    public ProductRepository(Context context)
    {
        this.context = context;
    }
    
    private IQueryable<Product> GetInstances()
    {
        return context.Products.Include(p => p.OrderProductsEntity).ThenInclude(op => op.OrderEntity);
    }
    public async Task<Guid> CreateProduct(Product product)
    {
        await context.AddAsync(product);
        await context.SaveChangesAsync();
        return product.Id;
    }

    public async Task DeleteProduct(Product product)
    {
        context.Remove(product);
        await context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await GetInstances().ToListAsync();
    }

    public async Task<Product?> GetProductById(Guid Id)
    {
        return await GetInstances().FirstOrDefaultAsync(p => p.Id == Id);
    }

    public async Task UpdateProduct(Product product)
    {
        await context.SaveChangesAsync();
    }
}