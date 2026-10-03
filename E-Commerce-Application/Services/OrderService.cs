using E_Commerce_Application.Data;
using E_Commerce_Application.DTOs.Order;
using E_Commerce_Application.Entities;
using E_Commerce_Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application.Services
{
	public class OrderService : IOrderService
	{
		private AppDbContext _dbContext;
		public OrderService(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<bool> CancelOrderAsync(int userId, int orderId)
		{
			try
			{
				var order = await _dbContext.Orders
			   .Include(o => o.OrderItems)
			   .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

				if (order == null)
					return false;

				if (order.Status != "Pending")
					throw new InvalidOperationException("Only pending orders can be cancelled.");

				foreach (var item in order.OrderItems)
				{
					var inventory = _dbContext.Inventory.FirstOrDefault(i => i.ProductId == item.ProductId);

					if (inventory != null)
					{
						inventory.QuantityAvailable += item.Quantity;
						inventory.UpdatedAt = DateTime.UtcNow;
					}
				}
				order.Status = "Cancelled";
				await _dbContext.SaveChangesAsync();

				return true;
			}
			catch(Exception e) { 
				throw new Exception(e.Message);
			}
			
		}

		public async Task<OrderResponseDto> CreateOrderAsync(int userId, CreateOrderDto dto)
		{
			await using var transaction = await _dbContext.Database.BeginTransactionAsync();
			try
			{
				var cart = await _dbContext.Carts
				.Include(c => c.CartItems)
				.ThenInclude(ci => ci.Product)
				.FirstOrDefaultAsync(c => c.UserId == userId);

				if (cart == null || !cart.CartItems.Any())
				{
					throw new InvalidOperationException("Cart is Empty");
				}

				// validate stock of each cart item
				foreach (var item in cart.CartItems)
				{
					var inventory = await _dbContext.Inventory.FirstOrDefaultAsync(i => i.ProductId == item.ProductId);

					if (inventory == null)
						throw new InvalidOperationException($"Inventory not found for product: {item.Product.Name}");

					if (item.Quantity > inventory.QuantityAvailable)
						throw new InvalidOperationException($"Insufficient stock for product: {item.Product.Name}");
				}

				// calculate total.
				var totalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

				var order = new Order
				{
					UserId = userId,
					OrderDate = DateTime.UtcNow,
					TotalAmount = totalAmount,
					Status = "Pending"
				};

				_dbContext.Orders.Add(order);

				// Create Order item

				foreach (var item in cart.CartItems)
				{
					var orderItem = new OrderItem
					{
						Order = order,
						ProductId = item.ProductId,
						Quantity = item.Quantity,
						UnitPrice = item.Product.Price,
					};

					_dbContext.OrderItems.Add(orderItem);

					// Reduce inventory
					var inventory = await _dbContext.Inventory
						.FirstAsync(i => i.ProductId == item.ProductId);

					inventory.QuantityAvailable -= item.Quantity;
					inventory.UpdatedAt = DateTime.UtcNow;
				}
				// Create address snapshot
				var orderAddress = new OrderAddress
				{
					Order = order,
					FullName = dto.FullName,
					AddressLine = dto.AddressLine,
					City = dto.City,
					State = dto.State,
					PostalCode = dto.PostalCode,
					Country = dto.Country
				};

				order.OrderAddress = orderAddress;

				// Clear cart
				_dbContext.CartItems.RemoveRange(cart.CartItems);

				cart.UpdatedAt = DateTime.UtcNow;

				await _dbContext.SaveChangesAsync();

				await transaction.CommitAsync();

				return await GetOrderByIdAsync(userId, order.Id) ?? throw new InvalidOperationException("Order could not be retrieved after creation.");

			}
			catch
			{
				await transaction.RollbackAsync();
				throw;
			}
		}

		public async Task<IEnumerable<OrderResponseDto>> GetMyOrdersAsync(int userId)
		{
			var orders = await _dbContext.Orders
				.Where(o => o.UserId == userId)
				.Include(o => o.OrderItems)
					.ThenInclude(oi => oi.Product)
				.Include(o => o.OrderAddress)
				.OrderByDescending(o => o.OrderDate)
				.ToListAsync();

			return orders.Select(MapToResponseDto);
		}

		public async Task<OrderResponseDto?> GetOrderByIdAsync(int userId, int orderId)
		{
			var order = await _dbContext.Orders
			   .Include(o => o.OrderItems)
				   .ThenInclude(oi => oi.Product)
			   .Include(o => o.OrderAddress)
			   .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

			if (order == null)
				return null;

			return MapToResponseDto(order);
		}

		private OrderResponseDto MapToResponseDto(Order order)
		{
			var items = order.OrderItems.Select(oi =>
				new OrderItemResponseDto
				{
					Id = oi.Id,
					ProductId = oi.ProductId,
					ProductName = oi.Product.Name,
					SKU = oi.Product.SKU,
					Quantity = oi.Quantity,
					UnitPrice = oi.UnitPrice,
					TotalPrice = oi.UnitPrice * oi.Quantity
				}).ToList();

			return new OrderResponseDto
			{
				Id = order.Id,
				OrderDate = order.OrderDate,
				TotalAmount = order.TotalAmount,
				Status = order.Status,

				Address = new OrderAddressResponseDto
				{
					Id = order.OrderAddress.Id,
					FullName = order.OrderAddress.FullName,
					AddressLine = order.OrderAddress.AddressLine,
					City = order.OrderAddress.City,
					State = order.OrderAddress.State,
					PostalCode = order.OrderAddress.PostalCode,
					Country = order.OrderAddress.Country
				},

				Items = items
			};
		}
	}
}
