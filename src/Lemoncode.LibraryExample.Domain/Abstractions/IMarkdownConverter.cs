using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Domain.Abstractions
{
	public interface IMarkdownConverter
	{

		string ConvertToHtml(string markdownText);
	}
}
