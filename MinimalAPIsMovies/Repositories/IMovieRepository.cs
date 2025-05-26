using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public interface IMovieRepository
	{
		Task Assign(int id, List<int> genresIds);
		Task Assign(int id, List<ActorMovie> actors);
		Task<int> Create(Movie movie);
		Task Delete(int id);
		Task<bool> Exists(int id);
		Task<IEnumerable<Movie>> GetAll(PaginationDTO paginationDTO);
		Task<Movie?> GetById(int id);
		Task Update(Movie movie);
	}
}