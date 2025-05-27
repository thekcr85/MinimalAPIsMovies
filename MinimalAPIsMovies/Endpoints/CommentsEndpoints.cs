using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Filters;
using MinimalAPIsMovies.Models;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Validations;

namespace MinimalAPIsMovies.Endpoints
{
	public static class CommentsEndpoints
	{
		public static RouteGroupBuilder MapComments(this RouteGroupBuilder group)
		{
			group.MapGet("/", GetComments).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("GetComments"));
			group.MapGet("/{id}", GetComment);
			group.MapPost("/", CreateComment).AddEndpointFilter<ValidationFilter<CreateCommentDTO>>();
			group.MapPut("/{id}", UpdateComment);
			group.MapDelete("/{id}", DeleteComment); 

			return group;
		}

		static async Task<Results<Ok<IEnumerable<CommentDTO>>, NotFound>> GetComments(int movieId, ICommentRepository commentRepository, IMovieRepository movieRepository, IMapper mapper)
		{
			if (!await movieRepository.Exists(movieId))
			{
				return TypedResults.NotFound();
			}
			
			var comments = await commentRepository.GetAll(movieId);
			var commentsDTO = mapper.Map<IEnumerable<CommentDTO>>(comments);
			return TypedResults.Ok(commentsDTO);
		}

		static async Task<Results<NotFound, Ok<CommentDTO>>> GetComment(int movieId, int id, IMovieRepository movieRepository, ICommentRepository commentRepository, IMapper mapper)
		{
			if (!await movieRepository.Exists(movieId))
			{
				return TypedResults.NotFound();
			}

			var comment = await commentRepository.GetById(id);
			if (comment == null)
			{
				return TypedResults.NotFound();
			}

			var commentDTO = mapper.Map<CommentDTO>(comment);
			return TypedResults.Ok(commentDTO);

		}

		static async Task<Results<Created<CommentDTO>, NotFound>> CreateComment(int movieId, CreateCommentDTO createCommentDTO, ICommentRepository commentRepository, IMovieRepository movieRepository, IMapper mapper, IOutputCacheStore outputCacheStore)
		{
			if (!await movieRepository.Exists(movieId))
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

		static async Task<Results<NotFound, NoContent>> UpdateComment(int movieId, int id, CreateCommentDTO createCommentDTO, ICommentRepository commentRepository, IMovieRepository movieRepository, IMapper mapper, IOutputCacheStore outputCacheStore)
		{
			if (!await movieRepository.Exists(movieId))
			{
				return TypedResults.NotFound();
			}

			if (!await commentRepository.Exists(id))
			{
				return TypedResults.NotFound();
			}

			var comment = mapper.Map<Comment>(createCommentDTO);
			comment.Id = id; // Ensure the ID is set for the update
			comment.MovieId = movieId; // Set the MovieId from the route parameter

			await commentRepository.Update(comment);
			await outputCacheStore.EvictByTagAsync("GetComments", default);
			return TypedResults.NoContent();
		}

		static async Task<Results<NotFound, NoContent>> DeleteComment(int movieId, int id, ICommentRepository commentRepository, IMovieRepository movieRepository, IMapper mapper, IOutputCacheStore outputCacheStore)
		{
			if (!await movieRepository.Exists(movieId))
			{
				return TypedResults.NotFound();
			}

			if (!await commentRepository.Exists(id))
			{
				return TypedResults.NotFound();
			}
			await commentRepository.Delete(id);
			await outputCacheStore.EvictByTagAsync("GetComments", default);
			return TypedResults.NoContent();
		}
	}
}
