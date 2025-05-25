using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MinimalAPIsMovies.Extensions
{
	public static class HttpContextExtensions
	{
		public static async Task InsertPaginationParameterInResponseHeader<T>(this HttpContext httpContext, IQueryable<T> queryable)
		{
			ArgumentNullException.ThrowIfNull(httpContext); // Ensure the HttpContext is not null
			ArgumentNullException.ThrowIfNull(queryable); // Ensure the query is not null

			var totalRecords = await queryable.CountAsync(); // Count the total number of records in the query
			httpContext.Response.Headers["X-Total-Count"] = totalRecords.ToString(); // Insert the total count of records into the response header
		}
	}
}
