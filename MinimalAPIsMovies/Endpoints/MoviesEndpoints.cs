using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Services;
using System.Runtime.CompilerServices;

namespace MinimalAPIsMovies.Endpoints
{
	public static class MoviesEndpoints
	{
		private readonly static string container = "movies";

		public static RouteGroupBuilder MapMovies(this RouteGroupBuilder group)
		{
			group.MapGet("/", GetMovies).CacheOutput(c => c.Expire(TimeSpan.FromMinutes(1)).Tag("GetMovies"));
			group.MapGet("/{id}", GetMovie);
			group.MapPost("/", CreateMovie).DisableAntiforgery();
			group.MapPut("/{id}", UpdateMovie).DisableAntiforgery();
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
	}
}
