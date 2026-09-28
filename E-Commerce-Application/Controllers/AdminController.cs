using E_Commerce_Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Application.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles ="Admin")]
	public class AdminController : ControllerBase
	{
		private readonly IUserService _userService;

		public AdminController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("users")]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _userService.GetAllUsersAsync();

			return Ok(users);
		}

		[HttpGet("users/{id}")]
		public async Task<IActionResult> GetUser(int id)
		{
			var user = await _userService.GetUserByIdAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			return Ok(user);
		}

		[HttpDelete("users/{id}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			var deleted = await _userService.DeleteUserAsync(id);

			if (!deleted)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}
