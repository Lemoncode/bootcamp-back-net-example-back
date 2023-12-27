using Lemoncode.LibraryExample.SsoServices.Abstractions;
using Lemoncode.LibraryExample.SsoServices.Config;

using Microsoft.Extensions.Options;

namespace Lemoncode.LibraryExample.SsoServices;

public class FrontendService(IOptionsSnapshot<FrontendConfig> frontendConfig, IHttpClientFactory httpClientFactory) : IFrontendService
{

	private readonly IOptionsSnapshot<FrontendConfig> _frontendConfig = frontendConfig;

	private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

	public async Task SignIn(string payload)
	{
		var response = await _httpClient.PostAsync(_frontendConfig.Value.FrontendSigninUrl, new StringContent(payload));
		response.EnsureSuccessStatusCode();
	}
}
