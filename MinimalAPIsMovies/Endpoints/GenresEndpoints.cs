using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;
using MinimalAPIsMovies.Repositories;
using System.Runtime.CompilerServices;

namespace MinimalAPIsMovies.Endpoints
{
	public static class GenresEndpoints
	{
		public static RouteGroupBuilder MapGenres(this RouteGroupBuilder routeGroupBuilder)
		{
			routeGroupBuilder.MapGet("/", GetGenres).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("GetGenres"));

			routeGroupBuilder.MapGet("/{id}", GetGenre);

			routeGroupBuilder.MapPost("/", CreateGenre);

			routeGroupBuilder.MapPut("/{id}", UpdateGenre);

			routeGroupBuilder.MapDelete("/{id}", DeleteGenre);

			return routeGroupBuilder;
		}

		static async Task<Ok<IEnumerable<GenreDTO>>> GetGenres(IGenreRepository genreRepository)
		{
			var genres = await genreRepository.GetAll();
			var genresDTO = genres.Select(g => new GenreDTO { Id = g.Id, Name = g.Name }); // Here we map the Genre model to GenreDTO
			return TypedResults.Ok(genresDTO);
		}

		static async Task<Results<Ok<GenreDTO>, NotFound>> GetGenre(int id, IGenreRepository genreRepository)
		{
			var genre = await genreRepository.GetById(id);

			if (genre == null)
			{
				return TypedResults.NotFound();
			}

			var genreDTO = new GenreDTO { Id = genre.Id, Name = genre.Name }; // Map model to DTO because we are returning DTO

			return TypedResults.Ok(genreDTO);
		}

		static async Task<Created<GenreDTO>> CreateGenre(CreateGenreDTO createGenreDTO, IGenreRepository genreRepository, IOutputCacheStore outputCacheStore)
		{
			var genre = new Genre { Name = createGenreDTO.Name }; // Map DTO to model
			var id = await genreRepository.Create(genre); // Create the genre and get the new ID
			await outputCacheStore.EvictByTagAsync("GetGenres", default); // Evict the cache for GetGenres
			var genreDTO = new GenreDTO { Id = id, Name = genre.Name }; // Map model to DTO for response
			return TypedResults.Created($"/genres/{id}", genreDTO);
		}

		static async Task<Results<NoContent, NotFound>> UpdateGenre(int id, CreateGenreDTO createGenreDTO, IGenreRepository genreRepository, IOutputCacheStore outputCacheStore)
		{
			var exists = await genreRepository.Exists(id);
			if (!exists)
			{
				return TypedResults.NotFound();
			}
			var genre = new Genre { Id = id, Name = createGenreDTO.Name }; // Map DTO to model with ID
			await genreRepository.Update(genre);
			await outputCacheStore.EvictByTagAsync("GetGenres", default);
			return TypedResults.NoContent();
		}

		static async Task<Results<NoContent, NotFound>> DeleteGenre(int id, IGenreRepository genreRepository, IOutputCacheStore outputCacheStore)
		{
			var exists = await genreRepository.Exists(id);
			if (!exists)
			{
				return TypedResults.NotFound();
			}
			await genreRepository.Delete(id);
			await outputCacheStore.EvictByTagAsync("GetGenres", default);
			return TypedResults.NoContent();
		}
	}
}
