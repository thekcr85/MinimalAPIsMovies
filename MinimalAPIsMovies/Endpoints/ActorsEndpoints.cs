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
		private readonly static string container = "actors"; // Container name for file storage

		public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
		{
			group.MapGet("/", GetActors).CacheOutput(c => c.Expire(TimeSpan.FromMinutes(1)).Tag("GetActors"));
			group.MapGet("/getActorsByName/{name}", GetActorsByName);
			group.MapGet("/{id}", GetActorById);
			group.MapPost("/", CreateActor).DisableAntiforgery();

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

		static async Task<Ok<IEnumerable<ActorDTO>>> GetActors(IActorRepository actorRepository, IMapper mapper, int page = 1, int recordsPerPage = 10)
		{
			var paginationDTO = new PaginationDTO
			{
				Page = page,
				RecordsPerPage = recordsPerPage
			};
			var actors = await actorRepository.GetAll(paginationDTO);
			var actorsDTO = mapper.Map<IEnumerable<ActorDTO>>(actors);
			return TypedResults.Ok(actorsDTO);
		}

		static async Task<Ok<IEnumerable<ActorDTO>>> GetActorsByName(string name, IActorRepository actorRepository, IMapper mapper)
		{
			var actors = await actorRepository.GetAllByName(name);
			var actorsDTO = mapper.Map<IEnumerable<ActorDTO>>(actors);
			return TypedResults.Ok(actorsDTO);
		}

		static async Task<Results<NotFound, Ok<ActorDTO>>> GetActorById(int id, IActorRepository actorRepository, IMapper mapper)
		{
			var actor = await actorRepository.GetById(id);
			if (actor == null)
			{
				return TypedResults.NotFound();
			}
			var actorDTO = mapper.Map<ActorDTO>(actor);
			return TypedResults.Ok(actorDTO);
		}
	}
}
