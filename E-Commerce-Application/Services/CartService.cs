using E_Commerce_Application.Data;
using E_Commerce_Application.DTOs.Cart;
using E_Commerce_Application.Entities;
using E_Commerce_Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application.Services
{
	public class CartService : ICartService
	{
		private readonly AppDbContext _context;

		public CartService(AppDbContext context) 
		{
			_context = context;
		}

		public async Task<CartResponseDto> AddToCart(int userId, AddCartItemDto dto)
		{
			// Check product
			var product = await _context.Products
				.FirstOrDefaultAsync(p =>
					p.Id == dto.ProductId &&
					p.IsActive);

			if (product == null)
				throw new KeyNotFoundException("Product not found.");

			// Check inventory
			var inventory = await _context.Inventory
				.FirstOrDefaultAsync(i =>
					i.ProductId == dto.ProductId);

			if (inventory == null)
				throw new InvalidOperationException(
					"Inventory not found for this product.");
			// Get or Create Cart
			var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

			if(cart == null)
			{
				cart = new Cart
				{
					UserId = userId,
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				};
				_context.Carts.Add(cart);
				await _context.SaveChangesAsync();
			}

			// Check whether product is already in cart
			var cartItem = await _context.CartItems
				.FirstOrDefaultAsync(ci =>
					ci.CartId == cart.Id &&
					ci.ProductId == dto.ProductId);

			var existingQuantity = cartItem?.Quantity ?? 0;

			if (existingQuantity + dto.Quantity >
				inventory.QuantityAvailable)
			{
				throw new InvalidOperationException(
					"Requested quantity exceeds available stock.");
			}
			if (cartItem != null)
			{
				cartItem.Quantity += dto.Quantity;
			}
			else
			{
				cartItem = new CartItem
				{
					CartId = cart.Id,
					ProductId = dto.ProductId,
					Quantity = dto.Quantity,
					AddedAt = DateTime.UtcNow
				};

				_context.CartItems.Add(cartItem);
			}
			cart.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			return await GetMyCartAsync(userId);
		}

		public async Task<bool> ClearCartAsync(int userId)
		{
			var cart = await _context.Carts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserId == userId);

			if (cart == null)
				return false;

			_context.CartItems.RemoveRange(cart.CartItems);

			cart.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<CartResponseDto> GetMyCartAsync(int userId)
		{
			var cart = await _context.Carts
				.Include(c => c.CartItems)
				.ThenInclude(c => c.Product)
				.FirstOrDefaultAsync(c=> c.UserId == userId);

			if (cart == null) 
			{
				cart = new Cart
				{
					UserId = userId,
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				};

				_context.Add(cart);
				await _context.SaveChangesAsync();
			}

			return MapToResponseDto(cart);
		}

		public async Task<bool> RemoveCartItemAsync(int userId, int cartItemId)
		{
			var cartItem = await _context.CartItems.FirstOrDefaultAsync(c=> c.Id == cartItemId && c.Cart.UserId == userId);

			if (cartItem == null)
				return false;

			cartItem.Cart.UpdatedAt = DateTime.UtcNow;
			_context.CartItems.Remove(cartItem);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<CartResponseDto?> UpdateCart(int userId, int cartItemId, UpdateCartItemDto dto)
		{
			var cartItem = await _context.CartItems
				.Include(c => c.Cart)
				.FirstOrDefaultAsync(c => c.Id == cartItemId && c.Cart.UserId == userId);

			if (cartItem == null) {
				return null;
			}

			var inventory = await _context.Inventory.FirstOrDefaultAsync(i => i.ProductId == cartItem.ProductId);

			if (inventory == null)
				throw new InvalidOperationException("Inventory not found for this product.");

			if (dto.Quantity > inventory.QuantityAvailable)
				throw new InvalidOperationException("Requested quantity exceeds available stock.");

			cartItem.Quantity = dto.Quantity;
			cartItem.Cart.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			return await GetMyCartAsync(userId);

		}

		private CartResponseDto MapToResponseDto(Cart cart)
		{
			var items = cart.CartItems.Select(ci =>
			new CartItemResponseDto
			{
				Id = ci.Id,
				ProductId = ci.ProductId,
				ProductName	= ci.Product.Name,
				SKU = ci.Product.SKU,
				UnitPrice = ci.Product.Price,
				Quantity = ci.Quantity,
				TotalPrice = ci.Product.Price * ci.Quantity,
				AddedAt = ci.AddedAt
			}).ToList();

			return new CartResponseDto { 
				Id = cart.Id,
				CreatedAt= cart.CreatedAt,
				UpdatedAt= cart.UpdatedAt,
				TotalAmount = items.Sum(i => i.TotalPrice),
				Items = items
			};
		}
	}
}
