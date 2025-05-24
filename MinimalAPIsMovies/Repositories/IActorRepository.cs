using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public interface IActorRepository
	{
		Task<int> Create(Actor actor);
		Task Delete(int id);
		Task<bool> Exists(int id);
		Task<IEnumerable<Actor>> GetAll();
		Task<Actor?> GetById(int id);
		Task Update(Actor actor);
	}
}