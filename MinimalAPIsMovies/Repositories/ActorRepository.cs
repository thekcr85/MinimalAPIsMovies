﻿using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Extensions;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public class ActorRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : IActorRepository
	{
		public async Task<IEnumerable<Actor>> GetAll(PaginationDTO paginationDTO)
		{
			var queryable = context.Actors.AsQueryable(); // Start with the base query
			await httpContextAccessor.HttpContext!.InsertPaginationParameterInResponseHeader(queryable);
			return await queryable.OrderBy(a => a.Name).Paginate(paginationDTO).ToListAsync(); // Order actors by name and fetch them as a list asynchronously
		}

		public async Task<Actor?> GetById(int id)
		{
			return await context.Actors.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id); // Fetch an actor by its ID without tracking changes,
																							 // because we are not updating it in this method,
																							 // so we can use AsNoTracking for better performance
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

		public async Task<List<int>> Exists(List<int> ids)
		{
			return await context.Actors.Where(a => ids.Contains(a.Id)).Select(a => a.Id).ToListAsync(); // Check if actors exist by their IDs and return the list of existing IDs
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

		public async Task<IEnumerable<Actor>> GetAllByName(string name)
		{
			return await context.Actors
				.Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) // Filter actors by name, case-insensitive
				.OrderBy(a => a.Name) // Order the results by name
				.ToListAsync(); // Fetch the results as a list asynchronously
		}
	}
}
