using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Filters;
using MinimalAPIsMovies.Models;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Services;
using System.Runtime.CompilerServices;

namespace MinimalAPIsMovies.Endpoints
{
	public static class MoviesEndpoints
	{
		private readonly static string container = "images/movies";

		public static RouteGroupBuilder MapMovies(this RouteGroupBuilder group)
		{
			group.MapGet("/", GetMovies).CacheOutput(c => c.Expire(TimeSpan.FromMinutes(1)).Tag("GetMovies"));
			group.MapGet("/{id}", GetMovie);
			group.MapPost("/", CreateMovie).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateMovieDTO>>();
			group.MapPut("/{id}", UpdateMovie).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateMovieDTO>>();
			group.MapDelete("/{id}", DeleteMovie);
			group.MapPost("/{id}/assignGenres", AssignGenres);
			group.MapPost("/{id}/assignActors", AssignActors);
			return group;
		}

		static async Task<Ok<IEnumerable<MovieDTO>>> GetMovies(IMovieRepository movieRepository, IMapper mapper, int page = 1, int recordsPerPage = 10)
		{
			var paginationDTO = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };
			var movies = await movieRepository.GetAll(paginationDTO);
			var moviesDTO = mapper.Map<IEnumerable<MovieDTO>>(movies);
			return TypedResults.Ok(moviesDTO);
		}

		static async Task<Results<NotFound, Ok<MovieDTO>>> GetMovie(int id, IMovieRepository movieRepository, IMapper mapper)
		{
			var movie = await movieRepository.GetById(id);
			if (movie is null)
			{
				return TypedResults.NotFound();
			}
			var movieDTO = mapper.Map<MovieDTO>(movie);
			return TypedResults.Ok(movieDTO);
		}

		static async Task<Created<MovieDTO>> CreateMovie([FromForm] CreateMovieDTO createMovieDTO, IMovieRepository movieRepository, IMapper mapper, IFileStorage fileStorage, IOutputCacheStore outputCacheStore)
		{
			var movie = mapper.Map<Movie>(createMovieDTO);
			if (createMovieDTO.Poster != null)
			{
				movie.Poster = await fileStorage.Store(container, createMovieDTO.Poster);
			}
			var id = await movieRepository.Create(movie);
			await outputCacheStore.EvictByTagAsync("GetMovies", default);
			var movieDTO = mapper.Map<MovieDTO>(movie);
			return TypedResults.Created($"/movies/{id}", movieDTO);
		}

		static async Task<Results<NotFound, NoContent>> UpdateMovie(int id, [FromForm] CreateMovieDTO createMovieDTO, IMovieRepository movieRepository, IMapper mapper, IFileStorage fileStorage, IOutputCacheStore outputCacheStore)
		{
			var movieDb = await movieRepository.GetById(id);
			if (movieDb is null)
			{
				return TypedResults.NotFound();
			}
			var movieForUpdate = mapper.Map(createMovieDTO, movieDb); // Map the DTO to the existing movie entity
			movieForUpdate.Id = id; // Ensure the ID is set correctly
			movieForUpdate.Poster = movieDb.Poster;
			if (createMovieDTO.Poster != null)
			{
				movieForUpdate.Poster = await fileStorage.Store(container, createMovieDTO.Poster); // Store the new poster if provided
			}
			await movieRepository.Update(movieForUpdate);
			await outputCacheStore.EvictByTagAsync("GetMovies", default); // Evict the cache for GetMovies
			return TypedResults.NoContent();
		}

		static async Task<Results<NotFound, NoContent>> DeleteMovie(int id, IMovieRepository movieRepository, IOutputCacheStore outputCacheStore, IFileStorage fileStorage)
		{
			var movieDb = await movieRepository.GetById(id);
			if (movieDb is null)
			{
				return TypedResults.NotFound();
			}
			await movieRepository.Delete(id);
			await fileStorage.Delete(movieDb.Poster, container); // Delete the poster file from storage
			await outputCacheStore.EvictByTagAsync("GetMovies", default); // Evict the cache for GetMovies
			return TypedResults.NoContent();
		}

		static async Task<Results<NotFound, NoContent, BadRequest<string>>> AssignGenres(int id, List<int> genresIds, IMovieRepository movieRepository, IGenreRepository genreRepository)
		{
			if (!await movieRepository.Exists(id))
			{
				return TypedResults.NotFound();
			}

			var existingGenresIds = new List<int>();

			if (genresIds.Count != 0)
			{
				existingGenresIds = await genreRepository.Exists(genresIds);
			}

			if (existingGenresIds.Count != genresIds.Count) // Check if all provided genre IDs exist
			{
				var notFoundIds = genresIds.Except(existingGenresIds); // Find the IDs that were not found
				return TypedResults.BadRequest($"The following genres do not exist: {string.Join(", ", notFoundIds)}"); // Return a BadRequest with the missing genre IDs
			}

			await movieRepository.Assign(id, existingGenresIds); // Assign the existing genre IDs to the movie
			return TypedResults.NoContent();
		}

		static async Task<Results<NotFound, NoContent, BadRequest<string>>> AssignActors(int id, List<AssignActorMovieDTO> actorsDTO, IMovieRepository movieRepository, IActorRepository actorRepository, IMapper mapper)
		{
			if (!await movieRepository.Exists(id)) // Check if the movie exists
			{
				return TypedResults.NotFound();
			}
			var existingActorsIds = new List<int>(); // Initialize a list to hold existing actor IDs
			var actorsIds = actorsDTO.Select(a => a.ActorId).ToList(); // Extract actor IDs from the DTOs

			if (actorsIds.Count != 0) // If there are actor IDs provided
			{
				existingActorsIds = await actorRepository.Exists(actorsIds); // Check which of the provided actor IDs exist in the database
			}

			if (existingActorsIds.Count != actorsDTO.Count) // Check if all provided actor IDs exist
			{
				var notFoundIds = actorsIds.Except(existingActorsIds); // Find the IDs that were not found
				return TypedResults.BadRequest($"The following actors do not exist: {string.Join(", ", notFoundIds)}"); // Return a BadRequest with the missing actor IDs
			}

			var actors = mapper.Map<List<ActorMovie>>(actorsDTO); // Map the DTOs to ActorMovie entities
			await movieRepository.Assign(id, actors); // Assign the actors to the movie
			return TypedResults.NoContent();
		}

	}
}
