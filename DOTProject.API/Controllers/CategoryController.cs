using System.Net;
using DOTProject.Application.Categories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DOTProject.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<CategoryModel> _categoryValidator;

        public CategoryController(ICategoryService categoryService, IValidator<CategoryModel> categoryValidator)
        {
            _categoryService = categoryService;
            _categoryValidator = categoryValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category {id} not found");
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryModel category)
        {
            ValidationResult result = await _categoryValidator.ValidateAsync(category);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            category = await _categoryService.AddAsync(category);
            return StatusCode((int)HttpStatusCode.Created, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryModel category)
        {

            if (id != category.Id)
            {
                return BadRequest("Category ID mismatch");
            }

            ValidationResult result = await _categoryValidator.ValidateAsync(category);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var isExists = await _categoryService.IsExistsAsync(id);
            if (!isExists) return NotFound($"Category {id} not found");

            category = await _categoryService.UpdateAsync(category);
            return StatusCode((int)HttpStatusCode.OK, category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isExists = await _categoryService.IsExistsAsync(id);
            if (!isExists) return NotFound($"Category {id} not found");

            var category = await _categoryService.DeleteAsync(id);
            return StatusCode((int)HttpStatusCode.OK, category);
        }
    }
}