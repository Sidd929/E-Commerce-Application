namespace E_Commerce_Application.DTOs.Order
{
	public class OrderAddressResponseDto
	{
		public int Id { get; set; }
		public string FullName { get; set; } = string.Empty;
		public string AddressLine { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string State { get; set; } = string.Empty;
		public string PostalCode { get; set; } = string.Empty;
		public string Country { get; set; } = string.Empty;
	}
}
