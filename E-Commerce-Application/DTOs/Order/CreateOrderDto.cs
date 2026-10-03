using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application.DTOs.Order
{
	public class CreateOrderDto
	{
		[Required]
		[StringLength(100)]
		public string FullName { get; set; } = string.Empty;

		[Required]
		[StringLength(250)]
		public string AddressLine { get; set; } = string.Empty;

		[Required]
		[StringLength(100)]
		public string City { get; set; } = string.Empty;

		[Required]
		[StringLength(100)]
		public string State { get; set; } = string.Empty;

		[Required]
		[StringLength(20)]
		public string PostalCode { get; set; } = string.Empty;

		[Required]
		[StringLength(100)]
		public string Country { get; set; } = string.Empty;
	}
}
