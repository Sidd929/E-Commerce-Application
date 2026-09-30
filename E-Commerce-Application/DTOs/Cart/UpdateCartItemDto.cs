using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application.DTOs.Cart
{
	public class UpdateCartItemDto
	{
		[Range(1, int.MaxValue)]
		public int Quantity { get; set; }
	}
}
