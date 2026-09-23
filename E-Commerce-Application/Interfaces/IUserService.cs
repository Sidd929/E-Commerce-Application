using E_Commerce_Application.DTOs.Users;
using E_Commerce_Application.Entities;

namespace E_Commerce_Application.Interfaces
{
	public interface IUserService
	{
		Task<UserResponseDto> CreateUserAsync(RegisterUserDto dto);

		Task<UserResponseDto?> GetUserByIdAsync(int id);

		Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto dto);

		Task<bool> DeleteUserAsync(int id);

		Task<UserResponseDto?> LoginAsync(LoginDto dto);
	}
}
