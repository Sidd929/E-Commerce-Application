namespace E_Commerce_Application.Entities
{
	public class Order
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.UtcNow;
		public decimal TotalAmount { get; set; }
		public string Status { get; set; } = "Pending";

		//navigation property
		public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

		public User User { get; set; } = null!;

		public OrderAddress OrderAddress { get; set; } = null!;
	}
}
