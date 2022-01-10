using Common.Persistence;
using Demo.Files.FilesManagement.Domain.Abstractions;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework;

public interface IFilesManagementTransactionalWriteContext
	: IFilesManagementWriteContext,
	  ITransaction
{
}