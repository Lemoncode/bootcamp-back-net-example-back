using Lemoncode.LibraryExample.Domain.Abstractions;

using Markdig;

namespace Lemoncode.LibraryExample.Infrastructure;

public class MarkdownConverter : IMarkdownConverter
{
	public string ConvertToHtml(string markdownText) => Markdown.ToHtml(markdownText);
}
