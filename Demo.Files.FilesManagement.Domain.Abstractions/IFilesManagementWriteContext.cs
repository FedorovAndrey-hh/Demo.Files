using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Abstractions;

public interface IFilesManagementWriteContext
{
	public Storage.IWriteContext ForStorage();
}