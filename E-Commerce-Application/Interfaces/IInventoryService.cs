using E_Commerce_Application.DTOs.Inventory;

namespace E_Commerce_Application.Interfaces
{
	public interface IInventoryService
	{
		Task<IEnumerable<InventoryResponseDto>> GetAllInventoryAsync();

		Task<InventoryResponseDto?> GetInventoryByProductIdAsync(int productId);

		Task<InventoryResponseDto> CreateInventoryAsync(CreateInventoryDto dto);

		Task<InventoryResponseDto?> UpdateInventoryAsync(int productId,UpdateInventoryDto dto);
	}
}
