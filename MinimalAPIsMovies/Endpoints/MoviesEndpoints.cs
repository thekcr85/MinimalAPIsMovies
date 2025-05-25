using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Repositories;
using System.Runtime.CompilerServices;

namespace MinimalAPIsMovies.Endpoints
{
	public static class MoviesEndpoints
	{
		public static RouteGroupBuilder MapMovies(this RouteGroupBuilder group)
		{
			return group;
		}

		static async Task<Ok<IEnumerable<MovieDTO>> GetMovies(IMovieRepository movieRepository, IMapper mapper, PaginationDTO paginationDTO)>
	}
}
