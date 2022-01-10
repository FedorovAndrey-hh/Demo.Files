namespace Common.Persistence;

public interface IPersistenceWiper
{
	public Task WiperAsync();
}