using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application.DTOs.Product
{
	public class UpdateProductDto
	{
		[Required]
		public int CategoryId { get; set; }

		[Required]
		[StringLength(150)]
		public string Name { get; set; } = string.Empty;

		[StringLength(1000)]
		public string Description { get; set; } = string.Empty;

		[Required]
		[StringLength(50)]
		public string SKU { get; set; } = string.Empty;

		[Range(0.01, double.MaxValue)]
		public decimal Price { get; set; }

		public bool IsActive { get; set; }
	}
}
