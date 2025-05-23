using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public interface IGenreRepository
	{
		Task<int> Create(Genre genre);
	}
}
