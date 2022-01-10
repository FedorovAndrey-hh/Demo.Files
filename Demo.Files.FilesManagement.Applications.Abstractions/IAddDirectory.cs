using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Applications.Abstractions;

public interface IAddDirectory
{
	public Task<(Storage, IDirectoryId)> ExecuteAsync(
		IStorageId storageId,
		StorageVersion? storageVersion,
		string directoryName);
}