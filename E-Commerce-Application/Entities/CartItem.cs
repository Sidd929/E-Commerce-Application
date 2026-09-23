namespace E_Commerce_Application.Entities
{
	public class CartItem
	{
		public int Id { get; set; }
		public int CartId { get; set; }
		public int ProductId {  get; set; }
		public int Quantity {  get; set; }
		public DateTime AddedAt {  get; set; }

		// Navigation Property
		public Cart Cart { get; set; } = null!;
		public Product Product { get; set; } = null!;
	}
}
