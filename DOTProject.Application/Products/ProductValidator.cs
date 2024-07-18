using FluentValidation;

namespace DOTProject.Application.Products
{
    public class ProductValidator : AbstractValidator<ProductModel>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(p => p.CategoryId).NotEmpty().NotNull().WithMessage("Category is required");
        }
    }
}