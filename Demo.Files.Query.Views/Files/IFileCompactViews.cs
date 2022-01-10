namespace Demo.Files.Query.Views.Files;

public interface IFileCompactViews
{
	public Task<string> FindByIdAsync(long storageId, long directoryId, long fileId);
}