namespace DOTProject.Application.Categories
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetAllAsync();
        Task<CategoryModel?> GetByIdAsync(int id);
        Task<CategoryModel?> AddAsync(CategoryModel model);
        Task<CategoryModel?> UpdateAsync(CategoryModel model);
        Task<CategoryModel?> DeleteAsync(int id);
        Task<bool> IsExistsAsync(int id);
        Task RefreshDataAsync();
    }
}