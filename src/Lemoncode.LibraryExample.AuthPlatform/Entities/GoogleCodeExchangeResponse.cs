﻿using System.Text.Json.Serialization;

namespace Lemoncode.LibraryExample.AuthPlatform.Entities
{
	public record class GoogleCodeExchangeResponse
	{

		[JsonPropertyName("access_token")]
		public required string AccessToken { get; set; }

		[JsonPropertyName("id_token")]
		public required string IdToken { get; set; }
		[JsonPropertyName("expires_in")]
		public required int ExpiresIn { get; set; }

		[JsonPropertyName("refresh_token")]
		public string? RefreshToken { get; set; }

		[JsonPropertyName("token_type")]
		public required string TokenType { get; set; }
	}
}
