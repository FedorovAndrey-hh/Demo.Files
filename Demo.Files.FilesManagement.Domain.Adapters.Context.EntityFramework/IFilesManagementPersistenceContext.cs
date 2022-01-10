namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework;

public interface IFilesManagementPersistenceContext
{
	public Task<IFilesManagementTransactionalWriteContext> BeginTransactionAsync();
}