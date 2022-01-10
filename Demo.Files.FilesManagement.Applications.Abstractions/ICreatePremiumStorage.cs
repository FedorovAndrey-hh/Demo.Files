using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Applications.Abstractions;

public interface ICreatePremiumStorage
{
	public Task<Storage> ExecuteAsync();
}