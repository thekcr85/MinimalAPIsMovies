using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Genre> Genres { get; set; }
		public DbSet<Actor> Actors { get; set; }
		public DbSet<Movie> Movies { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Call the base method to ensure the default behavior is applied
			// and then apply your custom configurations.
			{
				base.OnModelCreating(modelBuilder);

				modelBuilder.Entity<Genre>().Property(g => g.Name).HasMaxLength(150);

				modelBuilder.Entity<Actor>().Property(a => a.Name).HasMaxLength(150);
				modelBuilder.Entity<Actor>().Property(a => a.Picture).IsUnicode();

				modelBuilder.Entity<Movie>().Property(a => a.Title).HasMaxLength(250);
				modelBuilder.Entity<Movie>().Property(a => a.Poster).IsUnicode(); // Ensure that the Poster property is stored as a string in the database
			}
		}
	}
}
