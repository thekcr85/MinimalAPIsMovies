namespace MinimalAPIsMovies.Utilities
{
	public static class SigningKeysHandler
	{
		public const string DefaultIssuer = "my-app";
		private const string SigningKeysConfigSection = "Authentication:Schemes:Bearer:SigningKeys";
		private const string ConfigKeyIssuer = "Issuer";
		private const string ConfigKeyValue = "Value";
	}
}
