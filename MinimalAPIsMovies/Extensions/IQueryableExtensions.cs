using MinimalAPIsMovies.DTOs;

namespace MinimalAPIsMovies.Extensions
{
	public static class IQueryableExtensions
	{
		public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
		{
			return queryable
				.Skip((paginationDTO.Page - 1) * paginationDTO.RecordsPerPage) // Skip records based on the current page and records per page
																			   // // If the page is 1 and records per page is 10, it skips 0 records;
																			   // if page is 2, it skips 10 records, etc.
				.Take(paginationDTO.RecordsPerPage); // Take the number of records specified for the current page
		}
	}
}
