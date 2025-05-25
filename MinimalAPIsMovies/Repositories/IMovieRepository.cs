using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public interface IMovieRepository
	{
		Task<int> Create(Movie movie);
		Task Delete(int id);
		Task<bool> Exists(int id);
		Task<IEnumerable<Movie>> GetAll(PaginationDTO paginationDTO);
		Task<Movie?> GetById(int id);
		Task Update(Movie movie);
	}
}