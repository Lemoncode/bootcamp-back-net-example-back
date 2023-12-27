namespace Lemoncode.LibraryExample.Api.Abstractions
{
	public interface IJWTService
	{
		public string GenerateJwtToken(string familyName, string firstName, string emailAddress);

	}
}
