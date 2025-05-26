namespace MinimalAPIsMovies.Models
{
	public class ActorMovie
	{
		public int ActorId { get; set; } // Foreign key to Actor
		public int MovieId { get; set; } // Foreign key to Movie
		public Actor Actor { get; set; } = null!; // Navigation property to Actor
		public Movie Movie { get; set; } = null!; // Navigation property to Movie
		public int Order { get; set; } 
		public string Character { get; set; } = null!; 
	}
}
