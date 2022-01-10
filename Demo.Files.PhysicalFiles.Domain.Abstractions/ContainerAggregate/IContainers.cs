namespace Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;

public interface IContainers
{
	public Task<Container?> GetAsync(IContainerId id);

	public Task RemoveAsync(IContainerId id);
}