using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Services;

namespace MinimalAPIsMovies.Endpoints
{
	public static class ActorsEndpoints
	{
		private readonly static string container = "actors"; // Define the container for file storage

		public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
		{
			group.MapPost("/", CreateActor).DisableAntiforgery(); // Disable antiforgery for file uploads, because IFormFile is used
			return group;
		}

		static async Task<Created<ActorDTO>> CreateActor([FromForm] CreateActorDTO createActorDTO, IActorRepository actorRepository, IOutputCacheStore outputCacheStore, IMapper mapper, IFileStorage fileStorage)
		{
			var actor = mapper.Map<Actor>(createActorDTO);

			if (createActorDTO.Picture != null) 
			{
				actor.Picture = await fileStorage.Store(container, createActorDTO.Picture); // Store the picture file and set the Picture property
			}

			var id = await actorRepository.Create(actor); 
			await outputCacheStore.EvictByTagAsync("GetActors", default);
			var actorDTO = mapper.Map<ActorDTO>(actor);
			return TypedResults.Created($"/actors/{id}", actorDTO);
		}
	}
}
