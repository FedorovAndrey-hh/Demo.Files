using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Applications.Abstractions;

public interface IRemoveDirectory
{
	public Task<Storage> ExecuteAsync(
		IStorageId storageId,
		StorageVersion? storageVersion,
		IDirectoryId directoryId);
}