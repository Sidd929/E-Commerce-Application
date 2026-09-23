using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application.DTOs.Users
{
	public class RegisterUserDto
	{
		[Required]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		public string LastName { get; set; } = string.Empty;

		[Required]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string PhoneNumber { get; set; } = string.Empty;
		public string Role { get; set; } = "Customer";
		
		
	}
}
