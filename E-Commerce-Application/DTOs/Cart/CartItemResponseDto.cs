namespace E_Commerce_Application.DTOs.Cart
{
	public class CartItemResponseDto
	{
		public int Id { get; set; }

		public int ProductId { get; set; }

		public string ProductName { get; set; } = string.Empty;

		public string SKU { get; set; } = string.Empty;

		public decimal UnitPrice { get; set; }

		public int Quantity { get; set; }

		public decimal TotalPrice { get; set; }

		public DateTime AddedAt { get; set; }
	}
}
