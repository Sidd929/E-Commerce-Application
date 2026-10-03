using E_Commerce_Application.DTOs.Order;
using E_Commerce_Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_Application.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
		{
			try
			{
				var userId = GetUserId();

				var order = await _orderService.CreateOrderAsync(userId,dto);

				return Ok(order);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new
				{
					message = ex.Message
				});
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetMyOrders()
		{
			var userId = GetUserId();

			var orders = await _orderService.GetMyOrdersAsync(userId);

			return Ok(orders);
		}

		[HttpGet("{orderId}")]
		public async Task<IActionResult> GetOrderById(
			int orderId)
		{
			var userId = GetUserId();

			var order = await _orderService.GetOrderByIdAsync(userId,orderId);

			if (order == null)
				return NotFound(new
				{
					message = "Order not found."
				});

			return Ok(order);
		}

		[HttpDelete("{orderId}")]
		public async Task<IActionResult> CancelOrder(
		   int orderId)
		{
			try
			{
				var userId = GetUserId();

				var cancelled = await _orderService.CancelOrderAsync(userId,orderId);

				if (!cancelled)
					return NotFound(new
					{
						message = "Order not found."
					});

				return NoContent();
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new
				{
					message = ex.Message
				});
			}
		}

		private int GetUserId()
		{
			var userIdClaim = User.FindFirstValue(
				ClaimTypes.NameIdentifier);

			if (!int.TryParse(userIdClaim, out var userId))
				throw new UnauthorizedAccessException("Invalid user.");

			return userId;
		}
	}
}
