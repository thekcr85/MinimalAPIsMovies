namespace MinimalAPIsMovies.DTOs
{
	public class PaginationDTO
	{
		public int Page { get; set; } = 1; // Default to page 1
		private int _recordsPerPage = 10; // Default to 10 records per page
		private const int MaxRecordsPerPage = 50; // Maximum records per page limit

		public int RecordsPerPage
		{
			get => _recordsPerPage;
			set => _recordsPerPage = (value > MaxRecordsPerPage) ? MaxRecordsPerPage : value;  // Ensure that RecordsPerPage does not exceed the maximum limit
		}
	}

}
