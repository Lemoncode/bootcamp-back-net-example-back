using Lemoncode.LibraryExample.Domain.Abstractions;
using Lemoncode.LibraryExample.Domain.Entities.Books;

namespace Lemoncode.LibraryExample.Domain.Factories;

public class MarkdownValueFactory(IMarkdownConverter markdownConverter) : IMarkdownValueFactory
{

	private readonly IMarkdownConverter _markdownConverter = markdownConverter ?? throw new ArgumentNullException(nameof(markdownConverter));

	public BookDescription CreateBookDescription(string markdownText) => new BookDescription(_markdownConverter.ConvertToHtml(markdownText));

	public ReviewText CreateReviewText(string markdownText) => new ReviewText(_markdownConverter.ConvertToHtml(markdownText));
}
