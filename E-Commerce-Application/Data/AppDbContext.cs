using E_Commerce_Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application.Data
{
	public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
		{	
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Address> Addresses { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Inventory> Inventory { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<OrderAddress> OrderAddresses { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// User->Address
			modelBuilder.Entity<Address>()
				.HasOne(a => a.User)
				.WithMany(u => u.Addresses)
				.HasForeignKey(a => a.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Product>()
				.HasOne(c => c.Category)
				.WithMany(u => u.Products)
				.HasForeignKey(c=> c.CategoryId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Product>()
				.HasOne(i => i.Inventory)
				.WithOne(u => u.Product)
				.HasForeignKey<Inventory>(i => i.ProductId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Inventory>()
				.HasIndex(i => i.ProductId)
				.IsUnique();

			modelBuilder.Entity<Cart>()
				.HasOne(c => c.User)
				.WithOne(u => u.Cart)
				.HasForeignKey<Cart>(u => u.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<CartItem>()
				.HasOne(ci => ci.Cart)
				.WithMany(c => c.CartItems)
				.HasForeignKey(c => c.CartId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<CartItem>()
				.HasOne(p => p.Product)
				.WithMany(ci => ci.CartItems)
				.HasForeignKey(p => p.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Cart>()
				.HasIndex(c => c.UserId)
				.IsUnique();

			// User → Orders
			modelBuilder.Entity<Order>()
				.HasOne(o => o.User)
				.WithMany(u => u.Orders)
				.HasForeignKey(o => o.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			// Order -> OrderItem
			modelBuilder.Entity<OrderItem>()
				.HasOne(o => o.Order)
				.WithMany(oi => oi.OrderItems)
				.HasForeignKey(oi => oi.OrderId)
				.OnDelete(DeleteBehavior.Cascade);

			// Product → OrderItems
			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.Product)
				.WithMany()
				.HasForeignKey(oi => oi.ProductId)
				.OnDelete(DeleteBehavior.Restrict);


			// Order → OrderAddress
			modelBuilder.Entity<OrderAddress>()
				.HasOne(oa => oa.Order)
				.WithOne(o => o.OrderAddress)
				.HasForeignKey<OrderAddress>(oa => oa.OrderId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
