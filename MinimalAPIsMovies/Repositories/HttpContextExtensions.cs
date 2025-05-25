using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MinimalAPIsMovies.Repositories
{
	public static class HttpContextExtensions
	{
		public static async Task InsertPaginationParameterInResponseHeader<T>(this HttpContext httpContext, IQueryable<T> query)
		{
			ArgumentNullException.ThrowIfNull(httpContext); // Ensure the HttpContext is not null
			ArgumentNullException.ThrowIfNull(query); // Ensure the query is not null

			var totalRecords = await query.CountAsync(); // Count the total number of records in the query
			httpContext.Response.Headers["X-Total-Count"] = totalRecords.ToString(); // Insert the total count of records into the response header
		}
	}
}
