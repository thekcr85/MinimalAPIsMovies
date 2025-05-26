namespace MinimalAPIsMovies.Utilities
{
	public static class ValidationMessages
	{
		public const string NonEmpty = "The field {PropertyName} is required.";
		public const string MaximumLength = "The field {PropertyName} must not exceed {MaxLength} characters.";
		public const string FirstLetterIsUpperCase = "The first letter of the field {PropertyName} must be uppercase.";
		public static string GreaterThanDate(DateTime value) => "The field {PropertyName} must be greater than or equal to " + value.ToString("yyyy-MM-dd");
	}
}
