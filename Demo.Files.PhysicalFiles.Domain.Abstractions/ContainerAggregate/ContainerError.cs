namespace Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;

public enum ContainerError
{
	Unavailable,
	NotExists,
	ConcurrentAccess,

	FileNotExists
}