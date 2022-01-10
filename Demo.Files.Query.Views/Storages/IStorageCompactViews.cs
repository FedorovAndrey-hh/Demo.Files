namespace Demo.Files.Query.Views.Storages;

public interface IStorageCompactViews
{
	public Task<string?> FindByIdAsync(long id);
}