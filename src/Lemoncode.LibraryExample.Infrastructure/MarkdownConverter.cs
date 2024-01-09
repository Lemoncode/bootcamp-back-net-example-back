using Lemoncode.LibraryExample.Domain.Abstractions;

using Markdig;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Infrastructure;

public class MarkdownConverter : IMarkdownConverter
{
	public string ConvertToHtml(string markdownText) => Markdown.ToHtml(markdownText);
}
