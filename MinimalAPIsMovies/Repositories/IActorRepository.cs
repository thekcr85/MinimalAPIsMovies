using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public interface IActorRepository
	{
		Task<int> Create(Actor actor);
		Task Delete(int id);
		Task<bool> Exists(int id);
		Task<List<int>> Exists(List<int> ids);
		Task<IEnumerable<Actor>> GetAll(PaginationDTO paginationDTO);
		Task<IEnumerable<Actor>> GetAllByName(string name);
		Task<Actor?> GetById(int id);
		Task Update(Actor actor);
	}
}