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
		}
	}
}
