namespace E_Commerce_Application.DTOs.Users
{
	public class LoginResponseDto
	{
		public string Token { get; set; } = string.Empty;

		public UserResponseDto User { get; set; } = null!;
	}
}
