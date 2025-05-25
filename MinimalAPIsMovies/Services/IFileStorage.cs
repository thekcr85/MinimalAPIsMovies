namespace MinimalAPIsMovies.Services
{
	public interface IFileStorage
	{
		Task<string> Store(string container, IFormFile file);
		Task Delete(string? route, string container);
		Task<string> Edit(string? route, string container, IFormFile file);
	}
}
