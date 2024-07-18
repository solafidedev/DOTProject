using DOTProject.Application.Products;
using DOTProject.Domain.Entities;
using DOTProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DOTProject.Application.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const string CategoryCacheKey = "CategoryCache";

        public CategoryService(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync()
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            if (_cache.TryGetValue(CategoryCacheKey, out categories))
            {
                return categories;
            }
            categories = await _context.Categories.Include(p => p.Products)
                    .Select(s=> new CategoryModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Products = s.Products.Select(p => new ProductModel{
                            Id = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            CategoryId = p.CategoryId,
                        }).ToList()
                    })
                    .ToListAsync();

                _cache.Set(CategoryCacheKey, categories, TimeSpan.FromMinutes(5));
            return categories;
        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            var category = await _context.Categories.Include(p => p.Products).FirstOrDefaultAsync(p => p.Id == id);
            if(category is null) throw new KeyNotFoundException("Category not found");

            var result = new CategoryModel
            {
                Id = category.Id,
                Name = category.Name,
                Products = category.Products.Select(p=>new ProductModel{
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                }).ToList()
            };
            return result;
        }

        public async Task AddAsync(CategoryModel model)
        {
            Category category = new Category{
                Name = model.Name ?? "",
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoryModel model)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == model.Id);
            if(category is null) throw new KeyNotFoundException("Category not found");

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
            if(category is null) throw new KeyNotFoundException("Category not found");
            
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}