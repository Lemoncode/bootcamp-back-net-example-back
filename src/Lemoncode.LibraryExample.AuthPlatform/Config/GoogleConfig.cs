namespace Lemoncode.LibraryExample.AuthPlatform.Config;

public record class GoogleConfig
{
	public static readonly string ConfigSection = "GoogleSso";

	public required string OauthCodeUrl { get; set; }

	public required string OauthTokenUrl { get; set; }

	public required string ClientId { get; set; }

	public required string ClientSecret { get; set; }
	
	public required string RedirectUriForCode{ get; set; }
	
	public required string RedirectUriForToken { get; set; }

	public required string[] Scopes { get; set; }
}
