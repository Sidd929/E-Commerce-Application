namespace E_Commerce_Application.Entities
{
	public class Inventory
	{
		public int Id {  get; set; }
		public int ProductId { get; set; }
		public int QuantityAvailable {  get; set; }
		public int QuantityReserved {  get; set; }
		public int ReOrderLevel { get;set; }
		public DateTime UpdatedAt { get; set; }

		// Navigation Property
		public Product Product { get; set; } = null!;
	}
}
