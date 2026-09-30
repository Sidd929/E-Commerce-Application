using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application.DTOs.Cart
{
	public class AddCartItemDto
	{
		[Required]
		public int ProductId { get; set; }

		[Range(1, int.MaxValue)]
		public int Quantity { get; set; }
	}
}
