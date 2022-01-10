using Demo.Files.Query.Views.Files;

namespace Demo.Files.Query.Views.MongoDb.Files;

public sealed class FileDetailViews
	: IFileDetailViews
{
	public Task<string> FindByIdAsync(long storageId, long directoryId, long fileId)
	{
		throw new NotImplementedException();
	}
}