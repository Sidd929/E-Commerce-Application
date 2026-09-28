namespace E_Commerce_Application.Interfaces
{
	public interface IJwtInterface
	{
		string GenerateToken(int userId, string email, string role);
	}
}
