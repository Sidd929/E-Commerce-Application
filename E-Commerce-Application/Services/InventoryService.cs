using E_Commerce_Application.Data;
using E_Commerce_Application.DTOs.Inventory;
using E_Commerce_Application.Entities;
using E_Commerce_Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application.Services
{
	public class InventoryService : IInventoryService
	{
		private readonly AppDbContext _context;
		public InventoryService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<InventoryResponseDto> CreateInventoryAsync(CreateInventoryDto dto)
		{
			var product = await _context.Products
				.FirstOrDefaultAsync(p => p.Id == dto.ProductId);

			if (product == null)
				throw new KeyNotFoundException("Product not found.");

			// Check inventory already exists
			var inventoryExists = await _context.Inventory
				.AnyAsync(i => i.ProductId == dto.ProductId);

			if (inventoryExists)
				throw new InvalidOperationException(
					"Inventory already exists for this product.");

			var inventory = new Inventory
			{
				ProductId = dto.ProductId,
				QuantityAvailable = dto.QuantityAvailable,
				QuantityReserved = 0,
				ReOrderLevel = dto.ReOrderLevel,
				UpdatedAt = DateTime.UtcNow
			};

			_context.Inventory.Add(inventory);

			await _context.SaveChangesAsync();

			return (await GetInventoryByProductIdAsync(dto.ProductId))!;
		}

		public async Task<IEnumerable<InventoryResponseDto>> GetAllInventoryAsync()
		{
			return await _context.Inventory
				.Include(i => i.Product)
				.Select(i => new InventoryResponseDto
				{
					Id = i.Id,
					ProductId = i.ProductId,
					ProductName	= i.Product.Name,
					SKU = i.Product.SKU,
					QuantityAvailable = i.QuantityAvailable,
					QuantityReserved = i.QuantityReserved,
					ReOrderLevel = i.ReOrderLevel,
					UpdatedAt = i.UpdatedAt
				}).ToListAsync();
		}

		public async Task<InventoryResponseDto?> GetInventoryByProductIdAsync(int productId)
		{
			return await _context.Inventory
				.Include(i => i.Product)
				.Where(i => i.ProductId == productId)
				.Select(i => new InventoryResponseDto
				{
					Id = i.Id,
					ProductId = i.ProductId,
					ProductName = i.Product.Name,
					SKU = i.Product.SKU,
					QuantityAvailable = i.QuantityAvailable,
					QuantityReserved = i.QuantityReserved,
					ReOrderLevel = i.ReOrderLevel,
					UpdatedAt = i.UpdatedAt
				})
				.FirstOrDefaultAsync();
		}

		public async Task<InventoryResponseDto?> UpdateInventoryAsync(int productId, UpdateInventoryDto dto)
		{
			var inventory = await _context.Inventory
				.FirstOrDefaultAsync(i => i.ProductId == productId);

			if (inventory == null)
				return null;

			inventory.QuantityAvailable = dto.QuantityAvailable;
			inventory.ReOrderLevel = dto.ReOrderLevel;
			inventory.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			return await GetInventoryByProductIdAsync(productId);
		}
	}
}
