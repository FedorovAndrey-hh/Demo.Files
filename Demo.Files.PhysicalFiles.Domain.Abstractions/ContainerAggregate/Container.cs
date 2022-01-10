using Common.Core.Data;
using Common.Core.Diagnostics;
using Common.Core.Modifications;

namespace Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;

public sealed class Container : IIdentifiable<IContainerId>
{
	public static Task<Container> CreateAsync(IContext context)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		return context.CreateAsync();
	}

	private Container(IContainerId id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		Id = id;
	}

	public IContainerId Id { get; }

	public async Task<File> FileAsync(IContext context, IFileId id)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		return await context.GetFileAsync(Id, id).ConfigureAwait(false)
		       ?? throw new ContainerException(ContainerError.FileNotExists);
	}

	public async Task<IFileId> CreateFileAsync(IContext context, Func<Stream> data)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(data, nameof(data));

		IFileId? fileId = null;
		try
		{
			var createdFile = await context.CreateFileAsync(Id).ConfigureAwait(false);

			fileId = createdFile.Item1;
			await using var fileStream = createdFile.Item2;

			await using var dataStream = Contracts.NotNullReturnFrom(data(), nameof(data)) ;

			await dataStream.CopyToAsync(fileStream).ConfigureAwait(false);

			await fileStream.FlushAsync().ConfigureAwait(false);

			return fileId;
		}
		catch (Exception)
		{
			if (fileId is not null)
			{
				await context.DeleteFileIfExistsAsync(Id, fileId).ConfigureAwait(false);
			}

			throw;
		}
	}

	public async Task DeleteFileAsync(IContext context, IFileId id)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		if (!await context.DeleteFileIfExistsAsync(Id, id).ConfigureAwait(false))
		{
			throw new ContainerException(ContainerError.FileNotExists);
		}
	}

	public async Task<Stream> ReadFileAsync(IContext context, IFileId id)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		return await context.ReadFileAsync(Id, id).ConfigureAwait(false)
		       ?? throw new ContainerException(ContainerError.FileNotExists);
	}

	public async Task<Stream> WriteFileAsync(IContext context, IFileId id)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		return await context.WriteFileAsync(Id, id).ConfigureAwait(false)
		       ?? throw new ContainerException(ContainerError.FileNotExists);
	}

	public interface IContext
	{
		protected internal Task<Container> CreateAsync();
		protected internal Task<File?> GetFileAsync(IContainerId containerId, IFileId id);

		protected internal Task<(IFileId, Stream)> CreateFileAsync(IContainerId containerId);
		protected internal Task<bool> DeleteFileIfExistsAsync(IContainerId containerId, IFileId id);
		protected internal Task<bool> IsFileExistsAsync(IContainerId containerId, IFileId id);
		protected internal Task<Stream?> ReadFileAsync(IContainerId containerId, IFileId id);
		protected internal Task<Stream?> WriteFileAsync(IContainerId containerId, IFileId id);

		protected static Container RestoreContainer(IContainerId id)
		{
			Preconditions.RequiresNotNull(id, nameof(id));

			return new Container(id);
		}

		protected static File RestoreFile(IFileId id, DataSize<ulong> size) => new(id, size);
	}
}