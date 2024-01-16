namespace Lemoncode.LibraryExample.Domain.Entities.Exceptions;

[Serializable]
public class InvalidEntityStateException : Exception
{
	public InvalidEntityStateException(IList<string> validationErrors) : base(string.Join(Environment.NewLine, validationErrors)) { }

	public InvalidEntityStateException(string message) : base(message) { }
	public InvalidEntityStateException(string message, Exception inner) : base(message, inner) { }
}