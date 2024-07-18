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
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<IValidator<ProductModel>> _validatorMock;
        private readonly ProductController _productController;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _validatorMock = new Mock<IValidator<ProductModel>>();
            _productController = new ProductController(_productServiceMock.Object, _validatorMock.Object, _categoryServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithProducts()
        {
            // Arrange
            var products = new List<ProductModel> { new ProductModel { Id = 1, Name = "Product1" } };
            _productServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _productController.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<ProductModel>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _productServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ProductModel)null);

            // Act
            var result = await _productController.GetById(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var product = new ProductModel();
            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Name", "Name is required") });
            _validatorMock.Setup(validator => validator.ValidateAsync(product, default)).ReturnsAsync(validationResult);

            // Act
            var result = await _productController.Create(product);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var product = new ProductModel { CategoryId = 1 };
            var validationResult = new ValidationResult();
            _validatorMock.Setup(validator => validator.ValidateAsync(product, default)).ReturnsAsync(validationResult);
            _categoryServiceMock.Setup(service => service.IsExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _productController.Create(product);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenProductIDMismatch()
        {
            // Arrange
            var product = new ProductModel { Id = 1, Name = "Product1" };

            // Act
            var result = await _productController.Update(2, product);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var product = new ProductModel { Id = 1, CategoryId = 1, Name = "Product1" };
            var validationResult = new ValidationResult();
            _validatorMock.Setup(validator => validator.ValidateAsync(product, default)).ReturnsAsync(validationResult);
            _productServiceMock.Setup(service => service.IsExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _productController.Update(1, product);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var product = new ProductModel { Id = 1, CategoryId = 1, Name = "Product1" };
            var validationResult = new ValidationResult();
            _validatorMock.Setup(validator => validator.ValidateAsync(product, default)).ReturnsAsync(validationResult);
            _productServiceMock.Setup(service => service.IsExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            _categoryServiceMock.Setup(service => service.IsExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _productController.Update(1, product);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _productServiceMock.Setup(service => service.IsExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _productController.Delete(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenProductExists()
        {
            // Arrange
            var product = new ProductModel { Id = 1, Name = "Product1" };
            _productServiceMock.Setup(service => service.IsExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            _productServiceMock.Setup(service => service.DeleteAsync(It.IsAny<int>())).ReturnsAsync(product);

            // Act
            var result = await _productController.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductModel>(okResult.Value);
            Assert.Equal(product.Id, returnValue.Id);
        }
    }
}