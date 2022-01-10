using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Applications.Abstractions;

public interface IRelocateDirectory
{
	public Task<(Storage, IDirectoryId)> ExecuteAsync(
		IStorageId storageId,
		StorageVersion? storageVersion,
		IDirectoryId directoryId);
}