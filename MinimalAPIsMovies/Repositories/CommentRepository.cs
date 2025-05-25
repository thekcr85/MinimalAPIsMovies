using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Models;

namespace MinimalAPIsMovies.Repositories
{
	public class CommentRepository(ApplicationDbContext context) : ICommentRepository
	{
		public async Task<IEnumerable<Comment>> GetAll(int id)
		{
			return await context.Comments.Where(c => c.MovieId == id).ToListAsync();
		}

		public async Task<Comment?> GetById(int id)
		{
			return await context.Comments.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<int> Create(Comment comment)
		{
			context.Comments.Add(comment);
			await context.SaveChangesAsync();
			return comment.Id;
		}

		public async Task<bool> Exists(int id)
		{
			return await context.Comments.AnyAsync(c => c.Id == id);
		}

		public async Task Update(Comment comment)
		{
			context.Comments.Update(comment);
			await context.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			await context.Comments.Where(c => c.Id == id).ExecuteDeleteAsync();
		}
	}
}
