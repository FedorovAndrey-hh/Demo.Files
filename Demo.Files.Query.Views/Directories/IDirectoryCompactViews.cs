namespace Demo.Files.Query.Views.Directories;

public interface IDirectoryCompactViews
{
	public Task<string> FindAllByStorageIdAsync(long storageId);

	public Task<string?> FindByIdAsync(long storageId, long directoryId);
}