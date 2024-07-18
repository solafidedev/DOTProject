using DOTProject.Application.Categories;

namespace DOTProject.Application.Products
{
    public class ProductModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
    }
}