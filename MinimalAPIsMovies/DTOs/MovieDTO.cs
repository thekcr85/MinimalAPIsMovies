namespace MinimalAPIsMovies.DTOs
{
	public class MovieDTO
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public bool InTheaters { get; set; }
		public DateTime ReleaseDate { get; set; }
		public string? Poster { get; set; } // Nullable to allow for movies without posters
		public IEnumerable<CommentDTO> Comments { get; set; } = new List<CommentDTO>(); // Initialize to an empty list to avoid null reference issues
	}
}
