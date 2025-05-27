using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public interface IErrorRepository
	{
		Task Create(Error error);
	}
}