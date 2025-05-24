namespace MinimalAPIsMovies.DTOs
{
	public class CreateActorDTO
	{
		public string Name { get; set; } = null!;
		public DateTime DateOfBirth { get; set; }
		public IFormFile? Picture { get; set; } // IFormFile because it will be a file, not a string
	}
}
