namespace Lemoncode.LibraryExample.Application.Config
{
	public record class DapperConfig
	{
		public static readonly string ConfigurationSection = "ConnectionStrings";

		public required string DefaultConnectionString { get; set; }

	}
}
