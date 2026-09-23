using E_Commerce_Application.Data;
using E_Commerce_Application.DTOs.Users;
using E_Commerce_Application.Entities;
using E_Commerce_Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application.Services
{
	public class UserService : IUserService
	{
		private readonly AppDbContext _context;
		private readonly IPasswordHasher<User> _passwordHasher;

		public UserService(AppDbContext context, IPasswordHasher<User> passwordHasher)
		{
			_context = context;
			_passwordHasher = passwordHasher;
		}

		public async Task<UserResponseDto> CreateUserAsync(RegisterUserDto dto)
		{
			var email = dto.Email.Trim().ToLowerInvariant();
			var existingUser = await _context.Users.FirstOrDefaultAsync(u=> u.Email == email);

			if (existingUser != null)
			{
				throw new Exception("A user with this email already exists.");
			}

			var user = new User()
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Email = email,
				PhoneNumber = dto.PhoneNumber,
				Role = "Customer",
				CreatedAt = DateTime.UtcNow,
				IsActive = true
			};

			user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			var responseUser = new UserResponseDto()
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Role = user.Role,
				CreatedAt = user.CreatedAt,
				IsActive = user.IsActive
			};

			return responseUser;
		}

		public async Task<bool> DeleteUserAsync(int id)
		{
			var user = await _context.Users
				.FirstOrDefaultAsync(u => u.Id == id);

			if (user == null)
			{
				return false;
			}

			user.IsActive = false;

			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<UserResponseDto?> GetUserByIdAsync(int id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
			{
				return null;
			}
			var responseUser = new UserResponseDto()
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Role = user.Role,
				CreatedAt = user.CreatedAt,
				IsActive = user.IsActive
			};

			return responseUser;
		}

		public Task<UserResponseDto?> LoginAsync(LoginDto dto)
		{
			throw new NotImplementedException();
		}

		public async Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto dto)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null) { 
				return null;
			}
			user.FirstName = dto.FirstName;
			user.LastName = dto.LastName;
			user.PhoneNumber = dto.PhoneNumber;

			await _context.SaveChangesAsync();

			var responseUser = new UserResponseDto()
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Role = user.Role,
				CreatedAt = user.CreatedAt,
				IsActive = user.IsActive
			};

			return responseUser;
		}
	}
}
