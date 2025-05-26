using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Extensions;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public class MovieRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper) : IMovieRepository
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
			return await context.Movies
				.Include(m => m.Comments) // Include Comments navigation property
				.Include(m => m.GenresMovies).ThenInclude(gm => gm.Genre) // Include GenresMovies and Genre navigation properties
				.Include(m => m.ActorsMovies.OrderBy(am => am.Order)).ThenInclude(am => am.Actor) // Include ActorsMovies and Actor navigation properties
				.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
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

		public async Task Assign(int id, List<int> genresIds)
		{
			var movie = await context.Movies.Include(m => m.GenresMovies).FirstOrDefaultAsync(m => m.Id == id);

			if (movie == null)
			{
				throw new KeyNotFoundException($"Movie with ID {id} not found.");
			}

			var genresMovies = genresIds.Select(genreId => new GenreMovie { MovieId = id, GenreId = genreId }).ToList(); // Create a list of GenreMovie entities to be added
			movie.GenresMovies = mapper.Map(genresMovies, movie.GenresMovies); // Map the new genres to the existing GenresMovies collection
			await context.SaveChangesAsync();
		}

		public async Task Assign(int id, List<ActorMovie> actors)
		{
			for (int i = 1; i <= actors.Count; i++)
			{
				actors[i - 1].Order = i; // Set the order for each ActorMovie based on its position in the collection
			}

			var movie = await context.Movies.Include(m => m.ActorsMovies).FirstOrDefaultAsync(m => m.Id == id);

			if (movie == null)
			{
				throw new KeyNotFoundException($"Movie with ID {id} not found.");
			}

			movie.ActorsMovies = mapper.Map(actors, movie.ActorsMovies); // Map the new actors to the existing ActorsMovies collection
			await context.SaveChangesAsync();
		}
	}
}
