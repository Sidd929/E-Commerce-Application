using E_Commerce_Application.DTOs.Inventory;
using E_Commerce_Application.Interfaces;
using E_Commerce_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Application.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InventoryController : ControllerBase
	{
		private readonly IInventoryService inventoryService;
		
		public InventoryController(IInventoryService inventoryService)
		{
			this.inventoryService = inventoryService;
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllInventory() 
		{
			var inventory = await inventoryService.GetAllInventoryAsync();

			return Ok(inventory);
		}

		[HttpGet("{id}")]
		[Authorize(Roles ="Admin")]
		public async Task<IActionResult> GetInventoryByProductId(int id)
		{
			var inventory = await inventoryService.GetInventoryByProductIdAsync(id);
			if (inventory == null) { 
				return NotFound("Inventory not found");
			}
			return Ok(inventory);
		}

		[HttpPost]
		[Authorize(Roles ="Admin")]
		public async Task<IActionResult> CreateInventory(CreateInventoryDto dto)
		{
			try
			{
				var inventory = await inventoryService.CreateInventoryAsync(dto);
				return CreatedAtAction(
					nameof(GetInventoryByProductId),
					new { id = inventory.ProductId },
					inventory);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				return Conflict(ex.Message);
			}
		}

		[HttpPut("product/{productId}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateInventory(
			int productId,
			[FromBody] UpdateInventoryDto dto)
		{
			var inventory =
				await inventoryService.UpdateInventoryAsync(productId, dto);

			if (inventory == null)
				return NotFound("Inventory not found.");

			return Ok(inventory);
		}

	}
}
