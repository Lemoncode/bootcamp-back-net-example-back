namespace Lemoncode.LibraryExample.AuthPlatform.Abstractions
{
	public interface IJWTService
	{
		public string GenerateJwtToken(string familyName, string firstName, string emailAddress);

	}
}
