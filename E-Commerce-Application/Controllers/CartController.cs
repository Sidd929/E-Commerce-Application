using E_Commerce_Application.DTOs.Cart;
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
	public class CartController : ControllerBase
	{
		private readonly ICartService _cartService;

		public CartController(ICartService cartService)
		{
			_cartService = cartService;
		}

		[HttpGet]
		public async Task<IActionResult> GetMyCart()
		{
			int userId = GetUserId();
			var cart = await _cartService.GetMyCartAsync(userId);

			return Ok(cart);
		}

		[HttpPost]
		public async Task<IActionResult> AddToCart(AddCartItemDto dto)
		{
			try
			{
				var userId = GetUserId();
				var cart = await _cartService.AddToCart(userId, dto);

				return Ok(cart);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (InvalidOperationException ex) 
			{
				return BadRequest(new { message = ex.Message });
			}

		}

		[HttpPut]
		public async Task<IActionResult> UpdateCartItem(int cartItemId,UpdateCartItemDto dto)
		{
			try
			{
				var userId = GetUserId();

				var cart = await _cartService.UpdateCart(userId, cartItemId,dto);

				if (cart == null)
					return NotFound(new { message = "Cart item not found." });

				return Ok(cart);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpDelete("items/{cartItemId}")]
		public async Task<IActionResult> RemoveCartItem(int cartItemId)
		{
			int userId = GetUserId();
			bool isDeleted = await _cartService.RemoveCartItemAsync(userId, cartItemId);
			if (isDeleted)
			{
				return NoContent();
			}

			return NotFound(new { message = "Cart item not found." });
		}

		[HttpDelete]
		public async Task<IActionResult> ClearCart()
		{
			int userId = GetUserId() ;
			var cleared = await _cartService.ClearCartAsync(userId);

			if (!cleared)
				return NotFound(new { message = "Cart not found." });

			return NoContent();
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
