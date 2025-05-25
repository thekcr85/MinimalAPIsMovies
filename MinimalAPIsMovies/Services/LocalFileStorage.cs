namespace MinimalAPIsMovies.Services
{
	public class LocalFileStorage(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : IFileStorage
	{
		public async Task<string> Store(string container, IFormFile file)
		{
			var extension = Path.GetExtension(file.FileName); // Get the file extension
			var fileName = $"{Guid.NewGuid()}{extension}"; // Generate a unique file name using a GUID and the file extension
			var folder = Path.Combine(webHostEnvironment.WebRootPath, container); // Combine the web root path with the container name

			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			string route = Path.Combine(folder, fileName); // Combine the folder path with the file name
			using (var stream = new FileStream(route, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			var scheme = httpContextAccessor.HttpContext!.Request.Scheme; // Get the request scheme (http or https)
			var host = httpContextAccessor.HttpContext.Request.Host; // Get the request host (domain and port)
			var fileUrl = $"{scheme}://{host}/{container}/{fileName}"; // Construct the full URL for the stored file

			return fileUrl; // Return the full URL of the stored file
		}

		public Task Delete(string? route, string container)
		{
			if (string.IsNullOrEmpty(route))
			{
				return Task.CompletedTask;
			}

			var fileName = Path.GetFileName(route); // Extract the file name from the route
			var file = Path.Combine(webHostEnvironment.WebRootPath, container, fileName); // Combine the web root path with the container and file name

			if (File.Exists(file))
			{
				File.Delete(file); // Delete the file if it exists
			}

			return Task.CompletedTask; // Return a completed task
		}

		public Task<string> Edit(string? route, string container, IFormFile file)
		{
			// Implementation for editing the file locally
			throw new NotImplementedException();
		}
	}
}
