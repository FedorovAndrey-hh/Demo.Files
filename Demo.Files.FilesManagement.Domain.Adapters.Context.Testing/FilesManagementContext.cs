using Demo.Files.FilesManagement.Domain.Abstractions;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Demo.Files.FilesManagement.Domain.Adapters.Context.Testing.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.Testing;

public sealed class FilesManagementContext
	: IFilesManagementReadContext,
	  IFilesManagementWriteContext
{
	public FilesManagementContext(StorageContext? storage = null)
	{
		_storage = storage ?? new StorageContext();
	}

	private readonly StorageContext _storage;

	public IFilesManagementReadContext Read() => this;
	public IFilesManagementWriteContext Write() => this;

	Storage.IReadContext IFilesManagementReadContext.ForStorage() => _storage;

	Storage.IWriteContext IFilesManagementWriteContext.ForStorage() => _storage;
}