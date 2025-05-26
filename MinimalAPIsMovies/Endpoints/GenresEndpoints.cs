using AutoMapper;
using FluentValidation;
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
		public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
		{
			group.MapGet("/", GetGenres).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("GetGenres"));
			group.MapGet("/{id}", GetGenre);
			group.MapPost("/", CreateGenre);
			group.MapPut("/{id}", UpdateGenre);
			group.MapDelete("/{id}", DeleteGenre);

			return group;
		}

		static async Task<Ok<IEnumerable<GenreDTO>>> GetGenres(IGenreRepository genreRepository, IMapper mapper)
		{
			var genres = await genreRepository.GetAll();
			var genresDTO = mapper.Map<IEnumerable<GenreDTO>>(genres); // Use AutoMapper to map models to DTOs
			return TypedResults.Ok(genresDTO);
		}

		static async Task<Results<Ok<GenreDTO>, NotFound>> GetGenre(int id, IGenreRepository genreRepository, IMapper mapper)
		{
			var genre = await genreRepository.GetById(id);

			if (genre == null)
			{
				return TypedResults.NotFound();
			}

			var genreDTO = mapper.Map<GenreDTO>(genre); // Use AutoMapper to map model to DTO

			return TypedResults.Ok(genreDTO);
		}

		static async Task<Results<Created<GenreDTO>, ValidationProblem>> CreateGenre(CreateGenreDTO createGenreDTO, IGenreRepository genreRepository, IOutputCacheStore outputCacheStore, IMapper mapper, IValidator<CreateGenreDTO> validator)
		{
			var validationResult = await validator.ValidateAsync(createGenreDTO);
			if (!validationResult.IsValid)
			{
				return TypedResults.ValidationProblem(validationResult.ToDictionary());
			}
			var genre = mapper.Map<Genre>(createGenreDTO); // Use AutoMapper to map DTO to model
			var id = await genreRepository.Create(genre); // Create the genre and get the new ID
			await outputCacheStore.EvictByTagAsync("GetGenres", default); // Evict the cache for GetGenres
			var genreDTO = mapper.Map<GenreDTO>(genre); // Map the created genre to DTO
			return TypedResults.Created($"/genres/{id}", genreDTO);
		}

		static async Task<Results<NoContent, NotFound, ValidationProblem>> UpdateGenre(int id, CreateGenreDTO createGenreDTO, IGenreRepository genreRepository, IOutputCacheStore outputCacheStore, IMapper mapper, IValidator<CreateGenreDTO> validator)
		{
			var validationResult = await validator.ValidateAsync(createGenreDTO);

			if (!validationResult.IsValid)
			{
				return TypedResults.ValidationProblem(validationResult.ToDictionary());
			}

			var exists = await genreRepository.Exists(id);

			if (!exists)
			{
				return TypedResults.NotFound();
			}

			var genre = mapper.Map<Genre>(createGenreDTO); // Use AutoMapper to map DTO to model
			genre.Id = id; // Set the ID for the genre to update
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
