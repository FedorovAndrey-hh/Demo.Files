namespace Common.Persistence;

public interface IPersistenceInitializer
{
	public Task InitializeAsync();
}