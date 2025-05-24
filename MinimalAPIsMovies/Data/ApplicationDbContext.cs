using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Genre> Genres { get; set; } = null!; // DbSet property for the Genre entity
		public DbSet<Actor> Actors { get; set; } = null!; // DbSet property for the Actor entity

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Call the base method to ensure the default behavior is applied
			// and then apply your custom configurations.
			{
				base.OnModelCreating(modelBuilder);

				modelBuilder.Entity<Genre>().Property(g => g.Name).HasMaxLength(150);
			}
		}
	}
}
