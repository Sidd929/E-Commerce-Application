namespace E_Commerce_Application.DTOs.Order
{
	public class OrderResponseDto
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public string Status { get; set; } = string.Empty;

		public OrderAddressResponseDto Address { get; set; } = null!;

		public ICollection<OrderItemResponseDto> Items { get; set; }
			= new List<OrderItemResponseDto>();
	}
}
