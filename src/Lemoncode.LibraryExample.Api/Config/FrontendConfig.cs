namespace Lemoncode.LibraryExample.Api.Config;

public record class FrontendConfig
{
	public static readonly string ConfigSection = "Frontend";

	public required string FrontendBaseUrl { get; set; }
}
