namespace MinimalAPIsMovies.Models
{
	public class Actor
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public DateTime DateOfBirth { get; set; }
		public string? Picture { get; set; }
		public ICollection<ActorMovie> ActorsMovies { get; set; } = new List<ActorMovie>(); // Navigation property to ActorMovie
	}
}
