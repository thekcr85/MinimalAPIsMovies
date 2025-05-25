namespace MinimalAPIsMovies.Models
{
	public class GenreMovie
	{
		public int MovieId { get; set; }
		public int GenreId { get; set; }
		public Movie Movie { get; set; } // Navigation property to the Movie entity
		public Genre Genre { get; set; } // Navigation property to the Genre entity
	}
}
