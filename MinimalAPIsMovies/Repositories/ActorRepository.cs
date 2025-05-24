using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public class ActorRepository(ApplicationDbContext context) : IActorRepository
	{
		public async Task<IEnumerable<Actor>> GetAll()
		{
			return await context.Actors.OrderBy(a => a.Name).ToListAsync(); // Fetch all actors ordered by name
		}

		public async Task<Actor?> GetById(int id)
		{
			return await context.Actors.FindAsync(id); // Fetch an actor by its ID
		}

		public async Task<int> Create(Actor actor)
		{
			await context.AddAsync(actor);
			await context.SaveChangesAsync();
			return actor.Id; // Return the ID of the newly created actor
		}

		public async Task<bool> Exists(int id)
		{
			return await context.Actors.AnyAsync(a => a.Id == id); // Check if an actor exists by its ID
		}

		public async Task Update(Actor actor)
		{
			context.Update(actor); 
			await context.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			await context.Actors.Where(a => a.Id == id).ExecuteDeleteAsync();
		}  
	}
}
