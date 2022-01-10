namespace Demo.Files.Query.Views.Files;

public interface IFileDetailViews
{
	public Task<string> FindByIdAsync(long storageId, long directoryId, long fileId);
}