using E_Commerce_Application.Data;
using E_Commerce_Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace E_Commerce_Application.Services
{
	public class JwtService:IJwtInterface
	{
		private readonly IConfiguration _configuration;
		public JwtService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateToken(int userId, string email, string role)
		{
			var jwtKey = _configuration["Jwt:Key"];

			if (jwtKey == null) {
				throw new InvalidOperationException("Jwt Key was not configured");
			}
			var claims = new[] {
				new Claim(ClaimTypes.NameIdentifier,userId.ToString()),
				new Claim(ClaimTypes.Email,email),
				new Claim(ClaimTypes.Role,role)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

			var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(
					Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
