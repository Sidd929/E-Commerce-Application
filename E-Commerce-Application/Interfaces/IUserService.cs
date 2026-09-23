using E_Commerce_Application.Entities;

namespace E_Commerce_Application.Interfaces
{
	public interface IUserService
	{
		Task<User?> GetByIdAsync(int id);
		Task<User?> GetByEmailAsync(string email);
		Task<User> CreateUserAsync(User user);
	}
}
