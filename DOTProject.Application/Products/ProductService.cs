using DOTProject.Application.Categories;
using DOTProject.Domain.Entities;
using DOTProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DOTProject.Application.Products
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.Category).Select(s => new ProductModel
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                CategoryId = s.CategoryId,
                Category = new CategoryModel
                {
                    Id = s.Category.Id,
                    Name = s.Category.Name,
                },
            }).ToListAsync();
        }

        public async Task<ProductModel?> GetByIdAsync(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            var result = new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Category = new CategoryModel
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name
                }
            };
            return result;
        }

        public async Task<bool> IsExistsAsync(int id)
        {
            var isExists = await _context.Products.AnyAsync(p => p.Id == id);
            return isExists;
        }

        public async Task AddAsync(ProductModel model)
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price ?? 0,
                CategoryId = model.CategoryId ?? 0
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == model.Id);

            product.Name = model.Name;
            product.Price = model.Price ?? 0;
            product.CategoryId = model.CategoryId ?? 0;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}