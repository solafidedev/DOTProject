using DOTProject.API.Controllers;
using DOTProject.Application.Categories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DOTProject.UnitTests;
public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly Mock<IValidator<CategoryModel>> _categoryValidatorMock;
    private readonly CategoryController _categoryController;

    public CategoryControllerTests()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _categoryValidatorMock = new Mock<IValidator<CategoryModel>>();
        _categoryController = new CategoryController(_categoryServiceMock.Object, _categoryValidatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithCategories()
    {
        // Arrange
        var categories = new List<CategoryModel> { new CategoryModel { Id = 1, Name = "Category1" } };
        _categoryServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(categories);

        // Act
        var result = await _categoryController.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<CategoryModel>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        _categoryServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((CategoryModel)null);

        // Act
        var result = await _categoryController.GetById(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelIsInvalid()
    {
        // Arrange
        var category = new CategoryModel();
        var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Name", "Name is required") });
        _categoryValidatorMock.Setup(validator => validator.ValidateAsync(category, default)).ReturnsAsync(validationResult);

        // Act
        var result = await _categoryController.Create(category);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsOkResult_WhenModelIsValid()
    {
        // Arrange
        var category = new CategoryModel { Id = 1, Name = "Category1" };
        var validationResult = new ValidationResult();
        _categoryValidatorMock.Setup(validator => validator.ValidateAsync(category, default)).ReturnsAsync(validationResult);
        _categoryServiceMock.Setup(service => service.IsExistsAsync(category.Id ?? 0)).ReturnsAsync(true);
        _categoryServiceMock.Setup(service => service.UpdateAsync(category)).ReturnsAsync(category);

        // Act
        var result = await _categoryController.Update(1, category);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<CategoryModel>(okResult.Value);
        Assert.Equal(category.Id, returnValue.Id);
    }

    [Fact]
    public async Task Delete_ReturnsOkResult_WhenCategoryExists()
    {
        // Arrange
        var categoryId = 1;
        var category = new CategoryModel { Id = categoryId };
        _categoryServiceMock.Setup(service => service.IsExistsAsync(categoryId)).ReturnsAsync(true);
        _categoryServiceMock.Setup(service => service.DeleteAsync(categoryId)).ReturnsAsync(category);

        // Act
        var result = await _categoryController.Delete(categoryId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<CategoryModel>(okResult.Value);
        Assert.Equal(category, returnValue);
    }
}