namespace Lemoncode.LibraryExample.FileStorage;

public interface IFileRepository
{
	Stream GetFile(Uri fileUri);

	Task<Uri> UploadTempFile(Stream stream, string originalFileName);

	bool TempFileExists(Uri tempFileUri);

	Task<Uri> MoveFileToPermanentLocation(Uri tempFileUri, string permanentFileName);

	bool FileExists(Uri fileUri);

	void DeleteFile(Uri fileUri);
}