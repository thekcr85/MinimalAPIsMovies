using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Extensions;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public class MovieRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : IMovieRepository
	{
		public async Task<IEnumerable<Movie>> GetAll(PaginationDTO paginationDTO)
		{
			var queryable = context.Movies.AsQueryable();
			await httpContextAccessor.HttpContext!.InsertPaginationParameterInResponseHeader(queryable);
			return await queryable
				.OrderBy(m => m.Title)
				.Paginate(paginationDTO)
				.ToListAsync();
		}

		public async Task<Movie?> GetById(int id)
		{
			return await context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task<bool> Exists(int id)
		{
			return await context.Movies.AnyAsync(m => m.Id == id);
		}

		public async Task<int> Create(Movie movie)
		{
			context.Movies.Add(movie);
			await context.SaveChangesAsync();
			return movie.Id;
		}

		public async Task Update(Movie movie)
		{
			context.Movies.Update(movie);
			await context.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			await context.Movies.Where(m => m.Id == id).ExecuteDeleteAsync();
		}
	}
}
