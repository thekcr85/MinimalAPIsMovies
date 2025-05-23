using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public class GenreRepository : IGenreRepository
	{
		private readonly ApplicationDbContext _context;

		public GenreRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<int> Create(Genre genre)
		{
			await _context.AddAsync(genre);
			await _context.SaveChangesAsync();
			return genre.Id; // Return the ID of the newly created genre
		}

		public async Task<IEnumerable<Genre>> GetAll()
		{
			return await _context.Genres.OrderBy(g => g.Name).ToListAsync(); // Fetch all genres ordered by name
		}

		public async Task<Genre?> GetById(int id)
		{
			return await _context.Genres.FindAsync(id); // Fetch a genre by its ID
		}
	}
}
