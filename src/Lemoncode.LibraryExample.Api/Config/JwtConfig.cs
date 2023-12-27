namespace Lemoncode.LibraryExample.Api.Config
{
	public record class JwtConfig
	{
		public static readonly string ConfigSection = "JWT";

		public required string Audience { get; set; }

		public required string SigningKey { get; set; }

		public required string Issuer { get; set; }
	}
}
