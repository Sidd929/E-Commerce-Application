using E_Commerce_Application.DTOs.Cart;

namespace E_Commerce_Application.Interfaces
{
	public interface ICartService
	{
		Task<CartResponseDto> GetMyCartAsync(int userId);

		Task<CartResponseDto> AddToCart(int userId, AddCartItemDto dto);

		Task<CartResponseDto?> UpdateCart(int userId, int cartItemId, UpdateCartItemDto dto);

		Task<bool> RemoveCartItemAsync(int userId, int cartItemId);

		Task<bool> ClearCartAsync(int userId);
	}
}
