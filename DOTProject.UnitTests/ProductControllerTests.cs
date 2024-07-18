using DOTProject.API.Controllers;
using DOTProject.Application.Categories;
using DOTProject.Application.Products;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DOTProject.UnitTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly Mock<IValidator<ProductModel>> _mockValidator;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _mockValidator = new Mock<IValidator<ProductModel>>();
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new ProductController(_mockProductService.Object, _mockValidator.Object, _mockCategoryService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllProducts()
        {
            var products = new List<ProductModel>
            {
                new ProductModel { Id = 1, Name = "Laptop", Price = 1200.00m, CategoryId = 1 },
                new ProductModel { Id = 2, Name = "Headphones", Price = 100.00m, CategoryId = 1 }
            };

            _mockProductService.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

            var result = await _controller.GetAll() as OkObjectResult;
            var returnedProducts = result.Value as IEnumerable<ProductModel>;

            Assert.Equal(2, returnedProducts.Count());
            Assert.Equal("Laptop", returnedProducts.First().Name);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequestWhenValidationFails()
        {
            var product = new ProductModel { Id = 1, Name = "", Price = 1200.00m, CategoryId = 1 };
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Product name is required.")
            });

            _mockValidator.Setup(v => v.ValidateAsync(product, default)).ReturnsAsync(validationResult);

            var result = await _controller.Create(product);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtActionWhenSuccessful()
        {
            var product = new ProductModel { Id = 1, Name = "Laptop", Price = 1200.00m, CategoryId = 1 };
            var validationResult = new ValidationResult();

            _mockValidator.Setup(v => v.ValidateAsync(product, default)).ReturnsAsync(validationResult);
            _mockProductService.Setup(s => s.AddAsync(product)).Returns(Task.CompletedTask);

            var result = await _controller.Create(product) as CreatedAtActionResult;

            Assert.Equal("GetById", result.ActionName);
            Assert.Equal(1, ((ProductModel)result.Value).Id);
        }
    }
}