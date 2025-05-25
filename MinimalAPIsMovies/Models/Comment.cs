namespace MinimalAPIsMovies.Models
{
	public class Comment
	{
		public int Id { get; set; }
		public string Body { get; set; } = null!; // Body of the comment, cannot be null
		public int MovieId { get; set; } // Foreign key to the Movie entity
	}
}
