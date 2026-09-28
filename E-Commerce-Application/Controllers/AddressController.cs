using E_Commerce_Application.DTOs.Address;
using E_Commerce_Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_Application.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AddressController : ControllerBase
	{
		private readonly IAddressService _addressService;
		public AddressController(IAddressService addressService)
		{
			_addressService = addressService;
		}

		[HttpGet]
		public async Task<IActionResult> GetMyAddresses()
		{
			var userId = GetUserId();
			if (userId == null)
			{
				return Unauthorized();
			}
			var addresses = await _addressService.GetByUserIdAsync(userId.Value);

			return Ok(addresses);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetAddress(int id)
		{
			var userId = GetUserId();
			if (userId == null)
			{
				return Unauthorized();
			}
			var address = await _addressService.GetByIdAsync(id,userId.Value);
			if (address == null)
			{
				return NotFound();
			}

			return Ok(address);
		}

		[HttpPost]
		public async Task<IActionResult> CreateAddress(AddressDto dto)
		{
			var userId = GetUserId();
			if (userId == null)
			{
				return Unauthorized();
			}
			var address = await _addressService.CreateAsync(userId.Value, dto);

			return CreatedAtAction(
			   nameof(GetAddress),
			   new { id = address.Id },
			   address);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAddress(int id, AddressDto dto)
		{
			var userId = GetUserId();

			if (userId == null)
			{
				return Unauthorized();
			}

			var updated = await _addressService.UpdateAsync(
				id,
				userId.Value,
				dto);

			if (!updated)
			{
				return NotFound();
			}

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAddress(int id)
		{
			var userId = GetUserId();

			if (userId == null)
			{
				return Unauthorized();
			}

			var deleted = await _addressService.DeleteAsync(
				id,
				userId.Value);

			if (!deleted)
			{
				return NotFound();
			}

			return NoContent();
		}

		private int? GetUserId()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (int.TryParse(userId, out var id))
			{
				return id;
			}
			return null;
		}
	}
}
