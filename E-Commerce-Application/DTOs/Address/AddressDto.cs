using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application.DTOs.Address
{
	public class AddressDto
	{
		[Required]
		[MaxLength(200)]
		public string AddressLine1 { get; set; } = string.Empty;

		[MaxLength(200)]
		public string AddressLine2 { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string City { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string State { get; set; } = string.Empty;

		[Required]
		[MaxLength(20)]
		public string PostalCode { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string Country { get; set; } = string.Empty;

		public bool IsDefault { get; set; }
	}
}
