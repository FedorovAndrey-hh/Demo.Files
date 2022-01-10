using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Applications.Abstractions;

public interface IRenameDirectory
{
	public Task<Storage> ExecuteAsync(
		IStorageId storageId,
		StorageVersion? storageVersion,
		IDirectoryId directoryId,
		string newDirectoryName);
}