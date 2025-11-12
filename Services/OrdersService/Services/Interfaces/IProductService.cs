using BackEnd.DTOs;
using BackEnd.Entities;
using BackEnd.Share;

namespace BackEnd.Services;

public interface IProductService
{
    Task<Result<GetProduct>> GetEntityById(Guid id);
    Task<Result<List<GetProduct>>> GetProducts();
    Task<Result<Guid>> CreateProduct(PostProduct postProduct);
    Task<Result<bool>> UpdateProduct(PutProduct putProduct);
    Task<Result<bool>> DeleteProduct(DeleteProduct deleteProduct);
}