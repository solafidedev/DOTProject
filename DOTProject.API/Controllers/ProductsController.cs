using DOTProject.Application.Categories;
using DOTProject.Application.Products;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace DOTProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IValidator<ProductModel> _validator;

        public ProductController(IProductService productService, IValidator<ProductModel> validator, ICategoryService categoryService)
        {
            _productService = productService;
            _validator = validator;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product {id} not found");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductModel product)
        {
            ValidationResult result = await _validator.ValidateAsync(product);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            bool IsCategoryExists = await _categoryService.IsExistsAsync(product.CategoryId ?? 0);
            if(IsCategoryExists) NotFound($"Category {product.CategoryId ?? 0} not found");

            await _productService.AddAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductModel product)
        {
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch");
            }

            ValidationResult result = await _validator.ValidateAsync(product);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            bool isCategoryExists = await _categoryService.IsExistsAsync(product.CategoryId ?? 0);
            if(isCategoryExists) NotFound($"Category {product.CategoryId ?? 0} not found");

            await _productService.UpdateAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}