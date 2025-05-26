using System.Reflection.Metadata.Ecma335;

namespace MinimalAPIsMovies.Utilities
{
	public static class ValidationHelpers
	{
		public static bool FirstLetterIsUpperCase(string value)
		{
			return string.IsNullOrEmpty(value) || char.IsUpper(value[0]);
		}
	}
}
