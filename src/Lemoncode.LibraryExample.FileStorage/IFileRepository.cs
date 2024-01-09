namespace Lemoncode.LibraryExample.FileStorage.FileStorage;

public interface IFileRepository
{
	Stream GetFile(Uri fileUri);

	void DeleteImage(Uri imageUri);

	Task<string> UploadImageToTempFile(Stream stream);

	bool TempImageExists(string tempFileName);
}
