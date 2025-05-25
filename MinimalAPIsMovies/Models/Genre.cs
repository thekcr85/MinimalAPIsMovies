namespace MinimalAPIsMovies.Models
{
	public class Genre
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public ICollection<GenreMovie> GenresMovies { get; set; } = new List<GenreMovie>();
	}
}
