using Common.Core.Diagnostics;
using Demo.Files.FilesManagement.Domain.Abstractions;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework;

public sealed class FilesManagementReadContext : IFilesManagementReadContext
{
	public FilesManagementReadContext(StorageReadContext storage)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));

		_storage = storage;
	}

	private readonly StorageReadContext _storage;

	public Storage.IReadContext ForStorage() => _storage;
}