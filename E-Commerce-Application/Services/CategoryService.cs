using E_Commerce_Application.Data;
using E_Commerce_Application.DTOs.Category;
using E_Commerce_Application.Entities;
using E_Commerce_Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly AppDbContext _context;

		public CategoryService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<CategoryResponseDto> CreateAsync(CategoryDto dto)
		{
			var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == dto.Name);
			if (existingCategory != null)
			{
				throw new Exception("Category already exists.");
			}

			var category = new Category() { 
				Name = dto.Name,
				Description = dto.Description,
				IsActive = true
			};
			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			return new CategoryResponseDto
			{
				Id = category.Id,
				Name = category.Name,
				Description = category.Description
			};
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
			if (category == null) {
				return false;
			}
			category.IsActive = false;
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
		{
			return await _context.Categories
				.Where(c => c.IsActive)
				.Select(c => new CategoryResponseDto
				{
					Id = c.Id,
					Name = c.Name,
					Description = c.Description
				})
				.ToListAsync();
		}

		public async Task<CategoryResponseDto?> GetByIdAsync(int id)
		{
			return await _context.Categories
				.Where(c => c.Id == id && c.IsActive)
				.Select(c => new CategoryResponseDto
				{
					Id = c.Id,
					Name = c.Name,
					Description = c.Description
				})
				.FirstOrDefaultAsync();

		}

		public async Task<bool> UpdateAsync(int id, CategoryDto dto)
		{
			var category = await _context.Categories
				.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

			if (category == null)
			{
				return false;
			}

			category.Name = dto.Name;
			category.Description = dto.Description;

			await _context.SaveChangesAsync();

			return true;
		}
	}
}
