using System.Net.Http.Headers;
using System.Text.Json;
using Lemoncode.LibraryExample.AuthPlatform.Abstractions.IdentityProviders;
using Lemoncode.LibraryExample.AuthPlatform.Config.Microsoft;
using Lemoncode.LibraryExample.AuthPlatform.Entities.Microsoft;
using Lemoncode.LibraryExample.AuthPlatform.Exceptions;

using Microsoft.Extensions.Options;

namespace Lemoncode.LibraryExample.AuthPlatform.IdentityProviders;

public class MicrosoftOauthService : OauthService, IMicrosoftOauthService
{

	private HttpClient _httpClient;

	public MicrosoftOauthService(IOptionsSnapshot<MicrosoftOauthConfig> oauthConfig, IHttpClientFactory httpClientFactory) : base(oauthConfig, httpClientFactory)
	{
		_httpClient = httpClientFactory.CreateClient();
	}

	public async Task<GraphUserResponse> GetUserInfo(string token)
	{
		try
		{
			var message = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("https://graph.microsoft.com/v1.0/me")
			};

			message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var response = await _httpClient.SendAsync(message);
			string responseContent = default!;
			try
			{
				responseContent = await response.Content.ReadAsStringAsync();
			}
			catch
			{
			}
			if (!response.IsSuccessStatusCode)
			{
				throw new GraphException($"Error when retrieving user information from Graph. Error code: {response.StatusCode}. Message: {responseContent ?? "unknown"}.");
			}

			var jsonOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
			return JsonSerializer.Deserialize<GraphUserResponse>(responseContent, jsonOptions);
		}
		catch (Exception ex)
		{
			throw new InvalidTokenException("Error when validating and retrieving information from Microsoft's token.", ex);
		}
	}
}