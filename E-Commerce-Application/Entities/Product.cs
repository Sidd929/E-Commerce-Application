namespace E_Commerce_Application.Entities
{
	public class Product
	{
		public int Id { get; set; }
		//FK
		public int CategoryId { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;
		//Stock Keeping Unit - To manage different variant of same product
		public string SKU { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		// Navigation properties
		public Category Category { get; set; } = null!;
	}
}
