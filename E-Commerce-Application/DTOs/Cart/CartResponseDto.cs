namespace E_Commerce_Application.DTOs.Cart
{
	public class CartResponseDto
	{
		public int Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public decimal TotalAmount { get; set; }

		public ICollection<CartItemResponseDto> Items { get; set; }
		= new List<CartItemResponseDto>();
	}
}
