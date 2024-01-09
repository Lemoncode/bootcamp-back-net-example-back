namespace Lemoncode.LibraryExample.FileStorage.FileStorage;

public interface IFileRepository
{
	Stream GetFile(Uri fileUri);

	Task<Uri> UploadImageToTempFile(Stream stream, string originalFileName);

	bool TempFileExists(Uri tempFileUri);

	Task<Uri> CopyFileToPermanentLocation(Uri tempFileUri, string permanentFileName);

	bool FileExists(Uri fileUri);

	void DeleteFile(Uri fileUri);
}