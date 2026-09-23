using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application.DTOs.Users
{
	public class UpdateUserDto
	{
		[Required]
		[StringLength(50)]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[StringLength(50)]
		public string LastName { get; set; } = string.Empty;

		[Required]
		[Phone]
		public string PhoneNumber { get; set; } = string.Empty;
	}
}
