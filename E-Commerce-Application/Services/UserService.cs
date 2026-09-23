using E_Commerce_Application.Data;
using E_Commerce_Application.Entities;
using E_Commerce_Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application.Services
{
	public class UserService : IUserService
	{
		private readonly AppDbContext _context;
		public UserService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<User> CreateUserAsync(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return user;
		}

		public async Task<User?> GetByEmailAsync(string email)
		{
			return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
		}

		public async Task<User?> GetByIdAsync(int id)
		{
			return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
