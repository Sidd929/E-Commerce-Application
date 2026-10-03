using E_Commerce_Application.DTOs.Order;

namespace E_Commerce_Application.Interfaces
{
	public interface IOrderService
	{
		Task<OrderResponseDto> CreateOrderAsync(int userId, CreateOrderDto dto);

		Task<IEnumerable<OrderResponseDto>> GetMyOrdersAsync(int userId);

		Task<OrderResponseDto?> GetOrderByIdAsync(int userId, int orderId);

		Task<bool> CancelOrderAsync(int userId,int orderId);
	}
}
