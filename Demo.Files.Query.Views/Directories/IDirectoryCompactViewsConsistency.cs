using Common.Communication;
using Demo.Files.Common.Contracts.Communication.FilesManagement;

namespace Demo.Files.Query.Views.Directories;

public interface IDirectoryCompactViewsConsistency : IMessageHandler<StorageDirectoryAddedMessage>
{
}