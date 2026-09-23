namespace E_Commerce_Application.Entities
{
	public class OrderAddress
	{
		public int Id { get; set; }
		public int OrderId {  get; set; }
		public Order Order { get; set; } = null!;

		public string FullName { get; set; } = string.Empty;
		public string AddressLine { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string State { get; set; } = string.Empty;
		public string PostalCode { get; set; } = string.Empty;
		public string Country { get; set; } = string.Empty;
	}
}
