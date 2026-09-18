namespace E_Commerce_Application.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public string Role { get; set; } = "Customer";
		public DateTime CreatedAt { get; set; }
		public bool IsActive { get; set; }
		
		// Navigation Properties
		public ICollection<Address> Addresses { get; set; } = new List<Address>();
	}
}
