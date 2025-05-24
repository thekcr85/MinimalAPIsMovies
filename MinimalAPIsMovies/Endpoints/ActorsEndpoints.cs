using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;
using MinimalAPIsMovies.Repositories;

namespace MinimalAPIsMovies.Endpoints
{
	public static class ActorsEndpoints
	{
		public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
		{
			group.MapPost("/", CreateActor).DisableAntiforgery(); // Disable antiforgery for file uploads, because IFormFile is used
			return group;
		}

		static async Task<Created<ActorDTO>> CreateActor([FromForm] CreateActorDTO createActorDTO, IActorRepository actorRepository, IOutputCacheStore outputCacheStore, IMapper mapper)
		{
			var actor = mapper.Map<Actor>(createActorDTO); // Use AutoMapper to map DTO to model
			var id = await actorRepository.Create(actor); // Create the actor and get the new ID
			await outputCacheStore.EvictByTagAsync("GetActors", default); // Evict the cache for GetActors
			var actorDTO = mapper.Map<ActorDTO>(actor); // Map the created actor to DTO
			return TypedResults.Created($"/actors/{id}", actorDTO);
		}
	}
}
