using E_Commerce_Application.Data;
using E_Commerce_Application.DTOs.Product;
using E_Commerce_Application.Entities;
using E_Commerce_Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application.Services
{
	public class ProductService : IProductService
	{
		private readonly AppDbContext _context;
		public ProductService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto)
		{
			var belongingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.IsActive);
			if (belongingCategory == null) {
				throw new KeyNotFoundException("Category not found.");
			}

			// Check duplicate SKU
			var skuExists = await _context.Products
				.AnyAsync(p => p.SKU == dto.SKU);

			if (skuExists)
				throw new InvalidOperationException("SKU already exists.");

			var product = new Product()
			{
				CategoryId = dto.CategoryId,
				Name = dto.Name,
				Description = dto.Description,
				SKU = dto.SKU,
				Price = dto.Price,
				IsActive = true,
				CreatedAt = DateTime.UtcNow
			};

			_context.Products.Add(product);

			await _context.SaveChangesAsync();

			return (await GetProductByIdAsync(product.Id))!;
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			var product = await _context.Products
			   .FirstOrDefaultAsync(p => p.Id == id);

			if (product == null)
				return false;

			// Soft delete
			product.IsActive = false;
			product.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
		{
			return await _context.Products
				.Include(p => p.Category)
				.Where(p => p.IsActive)
				.Select(p => new ProductResponseDto
				{
					Id = p.Id,
					CategoryId = p.CategoryId,
					CategoryName = p.Category.Name,
					Name = p.Name,
					Description = p.Description,
					SKU = p.SKU,
					Price = p.Price,
					IsActive = p.IsActive,
					CreatedAt = p.CreatedAt,
					UpdatedAt = p.UpdatedAt
				})
				.ToListAsync();
		}

		public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
		{
			return await _context.Products
				.Include(p => p.Category)
				.Where(p => p.Id == id && p.IsActive)
				.Select(p => new ProductResponseDto {
					Id = p.Id,
					CategoryId = p.CategoryId,
					CategoryName = p.Category.Name,
					Name = p.Name,
					Description = p.Description,
					SKU = p.SKU,
					Price = p.Price,
					IsActive = p.IsActive,
					CreatedAt = p.CreatedAt,
					UpdatedAt = p.UpdatedAt
				})
				.FirstOrDefaultAsync();
		}

		public async Task<ProductResponseDto?> UpdateProductAsync(int id, UpdateProductDto dto)
		{
			var product = await _context.Products
				.FirstOrDefaultAsync(p => p.Id == id);

			if (product == null)
				return null;

			// Check category
			var categoryExists = await _context.Categories
				.AnyAsync(c => c.Id == dto.CategoryId && c.IsActive);

			if (!categoryExists)
				throw new KeyNotFoundException("Category not found.");

			// Check SKU belongs to another product
			var skuExists = await _context.Products
				.AnyAsync(p => p.SKU == dto.SKU && p.Id != id);

			if (skuExists)
				throw new InvalidOperationException("SKU already exists.");

			product.CategoryId = dto.CategoryId;
			product.Name = dto.Name;
			product.Description = dto.Description;
			product.SKU = dto.SKU;
			product.Price = dto.Price;
			product.IsActive = dto.IsActive;
			product.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			return await GetProductByIdAsync(id);
		}
	}
}
