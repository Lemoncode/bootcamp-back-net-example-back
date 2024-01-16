namespace Lemoncode.LibraryExample.Domain.Abstractions
{
	public interface IMarkdownConverter
	{

		string ConvertToHtml(string markdownText);
	}
}
