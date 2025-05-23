﻿using AutoMapper;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Utilities;

public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{
		CreateMap<Genre, GenreDTO>().ReverseMap(); // Maps Genre to GenreDTO and vice versa
		CreateMap<Genre, CreateGenreDTO>().ReverseMap();

		CreateMap<Actor, ActorDTO>().ReverseMap();
		CreateMap<Actor, CreateActorDTO>().ReverseMap()
			.ForMember(p => p.Picture, options => options.Ignore()); // Ignore the Picture property when mapping
																	 // from CreateActorDTO to Actor, as it will be handled separately
	}
}
