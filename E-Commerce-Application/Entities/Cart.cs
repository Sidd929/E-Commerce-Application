namespace E_Commerce_Application.Entities
{
	public class Cart
	{
		public int Id { get; set; }
		public int UserId {  get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		// Navigation Property
		public User User { get; set; } = null!;
		public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
	}
}
