using AutoMapper;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Utilities
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<Genre, GenreDTO>();
			CreateMap<CreateGenreDTO, Genre>();
			CreateMap<Genre, CreateGenreDTO>();
			CreateMap<GenreDTO, Genre>();
		}
	}
}
