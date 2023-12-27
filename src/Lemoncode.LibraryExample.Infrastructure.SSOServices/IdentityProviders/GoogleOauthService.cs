using Google.Apis.Auth;

using Lemoncode.LibraryExample.SsoServices.Abstractions;
using Lemoncode.LibraryExample.SsoServices.Config;
using Lemoncode.LibraryExample.SsoServices.Entities;
using Lemoncode.LibraryExample.SsoServices.Exceptions;

using Microsoft.Extensions.Options;

using System.Net;
using System.Text.Json;

namespace Lemoncode.LibraryExample.SsoServices.IdentityProviders;

public class GoogleOauthService : IGoogleOauthService
{

	private readonly IOptionsSnapshot<GoogleConfig> _googleConfig;

	private readonly HttpClient _httpClient;

	public GoogleOauthService(IOptionsSnapshot<GoogleConfig> googleConfig, IHttpClientFactory httpClientFactory)
	{
		_googleConfig = googleConfig;
		_httpClient = httpClientFactory.CreateClient();
	}

	public string GetOauthCodeUrl()
	{
		var dict = new Dictionary<string, string>
		{
			["client_id"] = _googleConfig.Value.ClientId,
			["redirect_uri"] = _googleConfig.Value.RedirectUriForCode,
			["response_type"] = "code",
			["scope"] = string.Join(' ', _googleConfig.Value.Scopes),
			["access_type"] = "online"
		};

		return GetUrlFromDictionary(_googleConfig.Value.OauthCodeUrl, dict);
	}

	private string GetUrlFromDictionary(string url, Dictionary<string, string> queryStringParams)
	{
		return $"{url}?{string.Join('&', queryStringParams.Select(d => $"{d.Key}={WebUtility.UrlEncode(d.Value)}"))}";
	}

	public async Task<GoogleCodeExchangeResponse> GetToken(string code)
	{
		var content = new FormUrlEncodedContent(new Dictionary<string, string>()
		{
			["client_id"] = _googleConfig.Value.ClientId,
			["client_secret"] = _googleConfig.Value.ClientSecret,
			["redirect_uri"] = _googleConfig.Value.RedirectUriForToken,
			["code"] = code,
			["grant_type"] = "authorization_code"
		});

		try
		{
			var response = await _httpClient.PostAsync(_googleConfig.Value.OauthTokenUrl, content);
			string? stringContent = null;
			try
			{
				stringContent = await response.Content.ReadAsStringAsync();
			}
			catch
			{
			}

			if (!response.IsSuccessStatusCode)
			{
				throw new GetTokenException($"Error when retrieving the token from Google. Error code: {response.StatusCode}. Error message: {stringContent ?? "Unknown"}");
			}

			if (string.IsNullOrWhiteSpace(stringContent))
			{
				throw new GetTokenException($"Error when retrieving the token from Google. No response from server. Response code: {response.StatusCode}.");
			}

			return JsonSerializer.Deserialize<GoogleCodeExchangeResponse>(stringContent!)!;
		}
		catch (Exception ex)
		{
			throw new GetTokenException("Error when retrieving the token from Google.", ex);
		}
	}

	public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string token)
	{
		try
		{
			var payload = await GoogleJsonWebSignature.ValidateAsync(token);
			return payload;
		}
		catch (Exception ex)
		{
			throw new InvalidTokenException("Error when validating and retrieving information from Google's token.", ex);
		}
	}
}