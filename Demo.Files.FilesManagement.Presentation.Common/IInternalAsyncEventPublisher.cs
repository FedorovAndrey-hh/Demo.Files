using Common.Core.Events;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Presentation.Common;

public interface IInternalAsyncEventPublisher
	: IAsyncEventPublisher<StorageEvent.Created>,
	  IAsyncEventPublisher<StorageEvent.Modified.DirectoryAdded>,
	  IAsyncEventPublisher<StorageEvent.Modified.DirectoryRelocated>,
	  IAsyncEventPublisher<StorageEvent.Modified.DirectoryRemoved>,
	  IAsyncEventPublisher<StorageEvent.Modified.DirectoryRenamed>
{
}