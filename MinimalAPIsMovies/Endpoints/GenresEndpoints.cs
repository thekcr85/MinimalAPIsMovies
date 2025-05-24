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

		static async Task<Ok<IEnumerable<Genre>>> GetGenres(IGenreRepository genreRepository)
		{
			var genres = await genreRepository.GetAll();
			return TypedResults.Ok(genres);
		}

		static async Task<Results<Ok<Genre>, NotFound>> GetGenre(int id, IGenreRepository genreRepository)
		{
			var genre = await genreRepository.GetById(id);
			return genre is not null ? TypedResults.Ok(genre) : TypedResults.NotFound();
		}

		static async Task<Created<Genre>> CreateGenre(CreateGenreDTO createGenreDTO, IGenreRepository genreRepository, IOutputCacheStore outputCacheStore)
		{
			var genre = new Genre { Name = createGenreDTO.Name }; // Map DTO to model
			var id = await genreRepository.Create(genre); // Create the genre and get the new ID
			await outputCacheStore.EvictByTagAsync("GetGenres", default); // Evict the cache for GetGenres
			return TypedResults.Created($"/genres/{id}", genre);
		}

		static async Task<Results<NoContent, NotFound>> UpdateGenre(int id, Genre genre, IGenreRepository genreRepository, IOutputCacheStore outputCacheStore)
		{
			var exists = await genreRepository.Exists(id);
			if (!exists)
			{
				return TypedResults.NotFound();
			}
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
