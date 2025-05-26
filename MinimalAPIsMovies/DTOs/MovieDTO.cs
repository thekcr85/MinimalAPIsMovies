namespace MinimalAPIsMovies.DTOs
{
	public class MovieDTO
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public bool InTheaters { get; set; }
		public DateTime ReleaseDate { get; set; }
		public string? Poster { get; set; }
		public IEnumerable<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
		public IEnumerable<GenreDTO> Genres { get; set; } = new List<GenreDTO>();
	}
}
