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

		public async Task<bool> Exists(int id)
		{
			return await _context.Genres.AnyAsync(g => g.Id == id); // Check if a genre exists by its ID
		}

		public async Task<List<int>> Exists(List<int> ids)
		{
			return await _context.Genres.Where(g => ids.Contains(g.Id)).Select(g => g.Id).ToListAsync(); // Check if genres exist by their IDs so we can return a list of existing IDs
		}

		public async Task Update(Genre genre)
		{
			_context.Update(genre); // Update the genre in the context
			await _context.SaveChangesAsync(); 
		}

		public async Task Delete(int id)
		{
			await _context.Genres.Where(g => g.Id == id).ExecuteDeleteAsync(); // Delete the genre by its ID
			await _context.SaveChangesAsync(); // Save changes to the database
		}
	}
}
