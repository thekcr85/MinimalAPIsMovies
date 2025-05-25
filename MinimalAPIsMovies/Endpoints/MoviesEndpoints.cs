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
			group.MapPost("/", CreateMovie).DisableAntiforgery();
			return group;
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
	}
}
