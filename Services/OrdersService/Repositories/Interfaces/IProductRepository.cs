using BackEnd.Entities;

namespace BackEnd.Repositories;

public interface IProductRepository
{
    Task<Product?> GetProductById(Guid Id);
    Task<List<Product>> GetAllProducts();
    Task<Guid> CreateProduct(Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(Product product);
}