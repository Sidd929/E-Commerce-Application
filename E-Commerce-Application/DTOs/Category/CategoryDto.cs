using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application.DTOs.Category
{
	public class CategoryDto
	{
		[Required]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;

		[MaxLength(500)]
		public string Description { get; set; } = string.Empty;
	}
}
