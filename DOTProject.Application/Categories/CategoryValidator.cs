using FluentValidation;

namespace DOTProject.Application.Categories
{
    public class CategoryValidator : AbstractValidator<CategoryModel>
    {
        public CategoryValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Category name is required.");
        }
    }
}