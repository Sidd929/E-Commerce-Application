using E_Commerce_Application.DTOs.Category;
using E_Commerce_Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Application.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var categories = await _categoryService.GetAllAsync();

			return Ok(categories);
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var category = await _categoryService.GetByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			return Ok(category);
		}

		[Authorize(Roles ="Admin")]
		[HttpPost]
		public async Task<IActionResult> Create(CategoryDto dto)
		{
			var category = await _categoryService.CreateAsync(dto);

			return CreatedAtAction(
				nameof(GetById),
				new { id = category.Id },
				category
			);
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, CategoryDto dto)
		{
			var updated = await _categoryService.UpdateAsync(id, dto);

			if (!updated)
			{
				return NotFound();
			}

			return NoContent();
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var deleted = await _categoryService.DeleteAsync(id);

			if (!deleted)
			{
				return NotFound();
			}

			return NoContent();
		}

	}
}
