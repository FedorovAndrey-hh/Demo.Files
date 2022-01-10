using Common.Communication;
using Demo.Files.Common.Contracts.Communication.FilesManagement;

namespace Demo.Files.Query.Views.Storages;

public interface IStorageCompactViewsConsistency
	: IMessageHandler<StorageCreatedMessage>,
	  IMessageHandler<StorageDirectoryAddedMessage>
{
}