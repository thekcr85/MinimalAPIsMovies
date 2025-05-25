namespace MinimalAPIsMovies.Services
{
	public class LocalFileStorage(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : IFileStorage
	{
		public async Task<string> Store(string container, IFormFile file)
		{
			var extension = Path.GetExtension(file.FileName);
			var fileName = $"{Guid.NewGuid()}{extension}";
			var folder = Path.Combine(webHostEnvironment.WebRootPath, container);

			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			var route = Path.Combine(folder, fileName);
			using (var stream = new FileStream(route, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			var context = httpContextAccessor.HttpContext
				?? throw new InvalidOperationException("HttpContext is not available");

			var scheme = context.Request.Scheme;
			var host = context.Request.Host;
			return $"{scheme}://{host}/{container}/{fileName}";
		}

		public Task Delete(string? route, string container)
		{
			if (string.IsNullOrEmpty(route)) return Task.CompletedTask;

			var fileName = Path.GetFileName(route);
			var file = Path.Combine(webHostEnvironment.WebRootPath, container, fileName);

			if (File.Exists(file)) File.Delete(file);

			return Task.CompletedTask;
		}

		public async Task<string> Edit(string? route, string container, IFormFile file)
		{
			await Delete(route, container);
			return await Store(container, file);
		}
	}

}
