using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public class ErrorRepository(ApplicationDbContext context) : IErrorRepository
	{
		public async Task Create(Error error)
		{
			await context.AddAsync(error);
			await context.SaveChangesAsync();
		}
	}
}
