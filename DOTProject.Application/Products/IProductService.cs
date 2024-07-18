using DOTProject.Domain.Entities;

namespace DOTProject.Application.Products
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetAllAsync();
        Task<ProductModel> GetByIdAsync(int id);
        Task AddAsync(ProductModel model);
        Task UpdateAsync(ProductModel model);
        Task DeleteAsync(int id);
    }
}