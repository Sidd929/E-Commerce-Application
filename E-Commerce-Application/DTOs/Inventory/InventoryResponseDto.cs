namespace E_Commerce_Application.DTOs.Inventory
{
	public class InventoryResponseDto
	{
		public int Id { get; set; }

		public int ProductId { get; set; }

		public string ProductName { get; set; } = string.Empty;

		public string SKU { get; set; } = string.Empty;

		public int QuantityAvailable { get; set; }

		public int QuantityReserved { get; set; }

		public int ReOrderLevel { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}
