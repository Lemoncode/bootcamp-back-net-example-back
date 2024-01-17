namespace Lemoncode.LibraryExample.AuthPlatform.Config;

public record class OauthConfig
{
	public required string OauthCodeUrl { get; set; }

	public required string OauthTokenUrl { get; set; }

	public required string ClientId { get; set; }

	public required string ClientSecret { get; set; }

	public required string RedirectUri { get; set; }

	public required string[] Scopes { get; set; }
}
