using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public interface ICommentRepository
	{
		Task<int> Create(Comment comment);
		Task Delete(int id);
		Task<bool> Exists(int id);
		Task<IEnumerable<Comment>> GetAll(int id);
		Task<Comment?> GetById(int id);
		Task Update(Comment comment);
	}
}