using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Abstractions;

public interface IFilesManagementReadContext
{
	public Storage.IReadContext ForStorage();
}