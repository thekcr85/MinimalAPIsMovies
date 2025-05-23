using Microsoft.EntityFrameworkCore;

namespace MinimalAPIsMovies.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
	}
}
