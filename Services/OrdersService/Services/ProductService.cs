using BackEnd.DTOs;
using BackEnd.Entities;
using BackEnd.Extensions;
using BackEnd.Repositories;
using BackEnd.Share;

namespace BackEnd.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;
    public ProductService(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }
    public async Task<Result<Guid>> CreateProduct(PostProduct postProduct)
    {
        Product product = postProduct.ToEntity();
        await productRepository.CreateProduct(product);
        return Result<Guid>.Success(product.Id);
    }

    public async Task<Result<bool>> DeleteProduct(DeleteProduct deleteProduct)
    {
        Product? product = await productRepository.GetProductById(deleteProduct.Id);
        if (product is null) return Result<bool>.Error(ErrorCode.StatusNotFound);
        await productRepository.DeleteProduct(product);
        return Result<bool>.Success(true);
    }

    public async Task<Result<GetProduct>> GetEntityById(Guid id)
    {
        Product? product = await productRepository.GetProductById(id);
        if (product is null) return Result<GetProduct>.Error(ErrorCode.StatusNotFound);
        return Result<GetProduct>.Success(product.ToDTO());
    }

    public async Task<Result<List<GetProduct>>> GetProducts()
    {
        List<Product> products = await productRepository.GetAllProducts();
        List<GetProduct> dtos = new List<GetProduct>();
        foreach (Product product in products)
        {
            dtos.Add(product.ToDTO());
        }
        return Result<List<GetProduct>>.Success(dtos);
    }

    public async Task<Result<bool>> UpdateProduct(PutProduct putProduct)
    {
        Product? product = await productRepository.GetProductById(putProduct.Id);
        if (product is null) return Result<bool>.Error(ErrorCode.StatusNotFound);
        product.Update(putProduct);
        await productRepository.UpdateProduct(product);
        return Result<bool>.Success(true);
    }
}