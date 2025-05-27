using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Genre> Genres { get; set; }
		public DbSet<Actor> Actors { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<GenreMovie> GenresMovies { get; set; } // DbSet for the join entity
		public DbSet<ActorMovie> ActorsMovies { get; set; }
		public DbSet<Error> Errors { get; set; }

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

				modelBuilder.Entity<GenreMovie>().HasKey(gm => new { gm.MovieId, gm.GenreId }); // Composite key for the join entity 

				modelBuilder.Entity<ActorMovie>().HasKey(am => new { am.ActorId, am.MovieId }); // Composite key for the join entity
			}
		}
	}
}
