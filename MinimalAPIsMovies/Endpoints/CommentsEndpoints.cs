using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;
using MinimalAPIsMovies.Repositories;

namespace MinimalAPIsMovies.Endpoints
{
	public static class CommentsEndpoints
	{
		public static RouteGroupBuilder MapComments(this RouteGroupBuilder group)
		{
			group.MapPost("/", CreateComment);
			return group;
		}

		static async Task<Results<Created<CommentDTO>, NotFound>> CreateComment(int movieId, CreateCommentDTO createCommentDTO, ICommentRepository commentRepository, IMovieRepository moviesRepository, IMapper mapper, IOutputCacheStore outputCacheStore)
		{
			if (!await moviesRepository.Exists(movieId))
			{
				return TypedResults.NotFound();
			}

			var comment = mapper.Map<Comment>(createCommentDTO);
			comment.MovieId = movieId; // Set the MovieId from the route parameter
			var id = await commentRepository.Create(comment); // Create the comment and get the new ID
			await outputCacheStore.EvictByTagAsync("GetComments", default);
			var commentDTO = mapper.Map<CommentDTO>(comment);
			return TypedResults.Created($"/movies/{movieId}/comments/{id}", commentDTO);
		}
	}
}
