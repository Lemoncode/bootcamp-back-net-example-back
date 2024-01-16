using Lemoncode.LibraryExample.Domain.Entities.Books;

namespace Lemoncode.LibraryExample.Domain.Abstractions;

public interface IMarkdownValueFactory
{

	BookDescription CreateBookDescription(string markdownText);
}
