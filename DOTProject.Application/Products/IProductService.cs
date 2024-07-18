namespace DOTProject.Application.Products
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetAllAsync();
        Task<ProductModel?> GetByIdAsync(int id);
        Task<ProductModel?> AddAsync(ProductModel model);
        Task<ProductModel?> UpdateAsync(ProductModel model);
        Task<ProductModel?> DeleteAsync(int id);
        Task<bool> IsExistsAsync(int id);
    }
}