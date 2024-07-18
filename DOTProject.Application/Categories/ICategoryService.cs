using DOTProject.Domain.Entities;

namespace DOTProject.Application.Categories
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetAllAsync();
        Task<CategoryModel> GetByIdAsync(int id);
        Task AddAsync(CategoryModel model);
        Task UpdateAsync(CategoryModel model);
        Task DeleteAsync(int id);
    }
}