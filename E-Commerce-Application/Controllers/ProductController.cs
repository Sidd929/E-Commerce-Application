using E_Commerce_Application.DTOs.Product;
using E_Commerce_Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Application.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;
		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetAllProducts()
		{
			var products = await _productService.GetAllProductsAsync();

			return Ok(products);
		}
		[HttpGet("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetProductById(int id)
		{
			var product = await _productService.GetProductByIdAsync(id);
			if (product == null)
				return NotFound("Product not found.");

			return Ok(product);
		}

		[HttpPut("{id}")]
		[Authorize(Roles ="Admin")]
		public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto product)
		{
			try
			{
				var updatedProduct = await _productService.UpdateProductAsync(id, product);
				if (product == null)
					return NotFound("Product not found.");

				return Ok(product);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				return Conflict(ex.Message);
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> CreateProduct(CreateProductDto dto)
		{
			try
			{
				var product = await _productService.CreateProductAsync(dto);

				return CreatedAtAction(
					nameof(GetProductById),
					new { id = product.Id },
					product);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				return Conflict(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var deleted = await _productService.DeleteProductAsync(id);

			if (!deleted)
				return NotFound("Product not found.");

			return NoContent();
		}
	}
}
