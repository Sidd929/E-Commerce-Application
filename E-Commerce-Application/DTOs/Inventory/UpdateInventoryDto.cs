using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application.DTOs.Inventory
{
	public class UpdateInventoryDto
	{
		[Range(0, int.MaxValue)]
		public int QuantityAvailable { get; set; }

		[Range(0, int.MaxValue)]
		public int ReOrderLevel { get; set; }
	}
}
