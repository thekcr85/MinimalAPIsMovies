namespace MinimalAPIsMovies.Models
{
	public class Movie
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public bool InTheaters { get; set; }
		public DateTime ReleaseDate { get; set; }
		public string? Poster { get; set; }
		public ICollection<Comment> Comments { get; set; } = new List<Comment>(); // Navigation property
		public ICollection<GenreMovie> GenresMovies { get; set; } = new List<GenreMovie>(); // Navigation property for the many-to-many relationship
	}
}
