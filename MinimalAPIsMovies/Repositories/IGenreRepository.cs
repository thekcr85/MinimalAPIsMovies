using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public interface IGenreRepository
	{
		Task<int> Create(Genre genre);
		Task<Genre?> GetById(int id);
		Task<IEnumerable<Genre>> GetAll();
		Task<bool> Exists(int id);
		Task Update(Genre genre);
	}
}
