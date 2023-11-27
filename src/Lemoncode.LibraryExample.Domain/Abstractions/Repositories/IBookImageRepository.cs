using Lemoncode.LibraryExample.Domain.Entities.Books;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.Domain.Abstractions.Repositories
{
	public interface IBookImageRepository
	{
		(Stream Stream, string FileName)  GetBookImage(int bookId);
		
		Task<string> UploadImageToTempFile(BookImageUpload bookImageUpload);

		void AssignImageToBook(int bookId, string tempFile);
		
		bool BookImageExists(string tempFileName);
	}
}
