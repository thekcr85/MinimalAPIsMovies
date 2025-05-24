using AutoMapper;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Utilities;

public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{
		CreateMap<Genre, GenreDTO>().ReverseMap();
		CreateMap<Genre, CreateGenreDTO>().ReverseMap();

		CreateMap<Actor, ActorDTO>().ReverseMap();
		CreateMap<Actor, CreateActorDTO>().ReverseMap();
	}
}
