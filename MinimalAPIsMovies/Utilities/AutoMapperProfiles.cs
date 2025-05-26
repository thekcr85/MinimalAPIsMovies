using AutoMapper;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Utilities;

public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{
		CreateMap<Genre, GenreDTO>();
		CreateMap<CreateGenreDTO, Genre>();

		CreateMap<Actor, ActorDTO>();
		CreateMap<CreateActorDTO, Actor>().ForMember(p => p.Picture, options => options.Ignore());

		CreateMap<Movie, MovieDTO>().ForMember(x => x.Genres, entity => entity.MapFrom(p => p.GenresMovies.Select(gm => new GenreDTO { Id = gm.GenreId, Name = gm.Genre.Name})));
		CreateMap<CreateMovieDTO, Movie>().ForMember(p => p.Poster, options => options.Ignore());
		
		CreateMap<Comment, CommentDTO>();
		CreateMap<CreateCommentDTO, Comment>();

		CreateMap<AssignActorMovieDTO, ActorMovie>();


	}
}
