namespace MinimalAPIsMovies.Services
{
	public interface IFileStorage
	{
		Task<string> Store(IFormFile file, string container);
		Task Delete(string fileName, string container);
		Task<string> Update(IFormFile file, string fileName, string container);
	}
}
