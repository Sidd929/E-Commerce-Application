using E_Commerce_Application.DTOs.Users;
using E_Commerce_Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Security.Claims;

namespace E_Commerce_Application.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterUserDto userDto)
		{
			var user = await _userService.CreateUserAsync(userDto);
			return Ok(user);
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginDto loginDto)
		{
			var result = await _userService.LoginAsync(loginDto);
			if (result == null)
			{
				return Unauthorized("Invalid email or password");
			}
			return Ok(result);
		}

		[Authorize]
		[HttpGet("me")]
		public async Task<IActionResult> GetMyProfile()
		{
			var userId = GetUserId();
			if (userId == null) {
				return Unauthorized();
			}
			var user = await _userService.GetUserByIdAsync(userId.Value);
			if (user == null) { 
				return NotFound();
			}
			return Ok(user);
		}
		[Authorize]
		[HttpPut("me")]
		public async Task<IActionResult> UpdateMyProfile(UpdateUserDto dto)
		{
			var userId = GetUserId();
			if (userId == null)
			{
				return Unauthorized();
			}

			var user = await _userService.UpdateUserAsync(userId.Value, dto);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user);
		}

		[Authorize]
		[HttpDelete("me")]
		public async Task<IActionResult> DeleteMyAccount()
		{
			var userId = GetUserId();

			if (userId == null)
			{
				return Unauthorized();
			}

			var deleted = await _userService.DeleteUserAsync(userId.Value);

			if (!deleted)
			{
				return NotFound();
			}

			return NoContent();
		}

		private int? GetUserId()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (int.TryParse(userId, out var Id))
			{
				return Id;
			}

			return null;
		}
	}
}
