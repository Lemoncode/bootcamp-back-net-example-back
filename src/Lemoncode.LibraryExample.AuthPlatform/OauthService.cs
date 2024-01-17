using Lemoncode.LibraryExample.AuthPlatform.Abstractions;
using Lemoncode.LibraryExample.AuthPlatform.Abstractions.IdentityProviders;
using Lemoncode.LibraryExample.AuthPlatform.Config;
using Lemoncode.LibraryExample.AuthPlatform.Entities;
using Lemoncode.LibraryExample.AuthPlatform.Exceptions;
using Microsoft.Extensions.Options;

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;

namespace Lemoncode.LibraryExample.AuthPlatform;

public class OauthService : IOauthService
{

	private readonly IOptionsSnapshot<OauthConfig> _oauthConfig;

	private readonly HttpClient _httpClient;

	public OauthService(IOptionsSnapshot<OauthConfig> oauthConfig, IHttpClientFactory httpClientFactory)
	{
		_oauthConfig = oauthConfig;
		_httpClient = httpClientFactory.CreateClient();
	}

	public string GetOauthCodeUrl(string? returnUrl = null)
	{
		var dict = new Dictionary<string, string>
		{
			["client_id"] = _oauthConfig.Value.ClientId,
			["redirect_uri"] = _oauthConfig.Value.RedirectUri,
			["response_type"] = "code",
			["scope"] = string.Join(' ', _oauthConfig.Value.Scopes),
			["access_type"] = "online"
		};
		if (!string.IsNullOrWhiteSpace(returnUrl))
		{
			dict.Add("state", returnUrl);
		}

		return GetUrlFromDictionary(_oauthConfig.Value.OauthCodeUrl, dict);
	}

	private string GetUrlFromDictionary(string url, Dictionary<string, string> queryStringParams)
	{
		return $"{url}?{string.Join('&', queryStringParams.Select(d => $"{d.Key}={WebUtility.UrlEncode(d.Value)}"))}";
	}

	public async Task<OauthCodeExchangeResponse> GetToken(string code)
	{
		var content = new FormUrlEncodedContent(new Dictionary<string, string>()
		{
			["client_id"] = _oauthConfig.Value.ClientId,
			["client_secret"] = _oauthConfig.Value.ClientSecret,
			["redirect_uri"] = _oauthConfig.Value.RedirectUri,
			["code"] = code,
			["grant_type"] = "authorization_code"
		});

		try
		{
			var response = await _httpClient.PostAsync(_oauthConfig.Value.OauthTokenUrl, content);
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
				throw new GetTokenException($"Error when retrieving the token. Error code: {response.StatusCode}. Error message: {stringContent ?? "Unknown"}");
			}

			if (string.IsNullOrWhiteSpace(stringContent))
			{
				throw new GetTokenException($"Error when retrieving the token. No response from server. Response code: {response.StatusCode}.");
			}

			return JsonSerializer.Deserialize<OauthCodeExchangeResponse>(stringContent!)!;
		}
		catch (Exception ex)
		{
			throw new GetTokenException("Error when retrieving the token.", ex);
		}
	}

	protected TPayload GetPayload<TPayload>(string token) where TPayload : class
	{
		try
		{
			var handler = new JwtSecurityTokenHandler();
			var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
			return JsonSerializer.Deserialize<TPayload>(jsonToken.Payload.SerializeToJson());
		}
		catch (Exception ex)
		{
			throw new InvalidTokenException("Error when validating and retrieving information from Google's token.", ex);
		}
	}
}