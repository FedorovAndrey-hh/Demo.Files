using Common.Core;
using Common.Core.Data;
using Common.Core.Diagnostics;
using Common.Core.Execution.Decoration;
using Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;
using File = Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate.File;
using TaskExtensions = Common.Core.TaskExtensions;

namespace Demo.Files.PhysicalFiles.Domain.Adapters.Context.Local.ContainerAggregate;

public sealed class ContainerContext
	: Container.IContext,
	  IContainers
{
	public ContainerContext(string rootDirectory)
	{
		Preconditions.RequiresNotNull(rootDirectory, nameof(rootDirectory));

		_rootDirectory = rootDirectory;
	}

	private string _GetContainerPath(ContainerId containerId)
		=> Path.Combine(_rootDirectory, containerId.Value.ToString());

	private string _GetFilePath(ContainerId containerId, FileId fileId)
		=> Path.Combine(_GetContainerPath(containerId), fileId.Value.ToString());

	private readonly string _rootDirectory;

	private void _CheckAvailability()
	{
		if (!Directory.Exists(_rootDirectory))
		{
			throw new ContainerException(ContainerError.Unavailable);
		}
	}

	Task<Container> Container.IContext.CreateAsync()
	{
		_CheckAvailability();

		return Container.IContext.RestoreContainer(ContainerId.Create()).AsTaskResult<Container>();
	}

	Task<File?> Container.IContext.GetFileAsync(IContainerId containerId, IFileId id)
	{
		Preconditions.RequiresNotNull(containerId, nameof(containerId));
		Preconditions.RequiresNotNull(id, nameof(id));

		try
		{
			var fileInfo = new FileInfo(_GetFilePath(containerId.Concrete(), id.Concrete()));
			return Container.IContext.RestoreFile(id, DataSize.Bytes((ulong)fileInfo.Length)).AsTaskResult<File?>();
		}
		catch (FileNotFoundException)
		{
			return Task.FromResult<File?>(null);
		}
		catch (Exception e) when (_ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw _ExceptionWrapper.Wrap(e);
		}
	}

	Task<(IFileId, Stream)> Container.IContext.CreateFileAsync(IContainerId containerId)
	{
		Preconditions.RequiresNotNull(containerId, nameof(containerId));

		var fileId = FileId.Create();

		var typedContainerId = containerId.Concrete();
		var directoryPath = _GetContainerPath(typedContainerId);
		var filePath = _GetFilePath(typedContainerId, fileId);

		try
		{
			Directory.CreateDirectory(directoryPath);
		}
		catch (Exception e) when (_ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw _ExceptionWrapper.Wrap(e);
		}

		try
		{
			var fileStream = new FileStream(
				filePath,
				FileMode.CreateNew,
				FileAccess.Write,
				FileShare.None,
				bufferSize: 4096,
				useAsync: true
			);
			return (fileId, fileStream).AsTaskResult<(IFileId, Stream)>();
		}
		catch (Exception e) when (_ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw _ExceptionWrapper.Wrap(e);
		}
	}

	Task<bool> Container.IContext.DeleteFileIfExistsAsync(IContainerId containerId, IFileId id)
	{
		Preconditions.RequiresNotNull(containerId, nameof(containerId));
		Preconditions.RequiresNotNull(id, nameof(id));

		try
		{
			var filePath = _GetFilePath(containerId.Concrete(), id.Concrete());

			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);

				return TaskExtensions.TrueResult;
			}
			else
			{
				return TaskExtensions.FalseResult;
			}
		}
		catch (Exception e) when (_ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw _ExceptionWrapper.Wrap(e);
		}
	}

	Task<bool> Container.IContext.IsFileExistsAsync(IContainerId containerId, IFileId id)
	{
		Preconditions.RequiresNotNull(containerId, nameof(containerId));
		Preconditions.RequiresNotNull(id, nameof(id));

		try
		{
			return System.IO.File.Exists(_GetFilePath(containerId.Concrete(), id.Concrete())).AsTaskResult();
		}
		catch (Exception e) when (_ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw _ExceptionWrapper.Wrap(e);
		}
	}

	Task<Stream?> Container.IContext.ReadFileAsync(IContainerId containerId, IFileId id)
	{
		Preconditions.RequiresNotNull(containerId, nameof(containerId));
		Preconditions.RequiresNotNull(id, nameof(id));

		try
		{
			return new FileStream(
					_GetFilePath(containerId.Concrete(), id.Concrete()),
					FileMode.Open,
					FileAccess.Read,
					FileShare.Read,
					bufferSize: 4096,
					useAsync: true
				)
				.AsTaskResult<Stream?>();
		}
		catch (FileNotFoundException)
		{
			return Task.FromResult<Stream?>(null);
		}
		catch (Exception e) when (_ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw _ExceptionWrapper.Wrap(e);
		}
	}

	Task<Stream?> Container.IContext.WriteFileAsync(IContainerId containerId, IFileId id)
	{
		Preconditions.RequiresNotNull(containerId, nameof(containerId));
		Preconditions.RequiresNotNull(id, nameof(id));

		try
		{
			return
				new FileStream(
						_GetFilePath(containerId.Concrete(), id.Concrete()),
						FileMode.OpenOrCreate,
						FileAccess.Write,
						FileShare.None,
						bufferSize: 4096,
						useAsync: true
					)
					.AsTaskResult<Stream?>();
		}
		catch (FileNotFoundException)
		{
			return Task.FromResult<Stream?>(null);
		}
		catch (Exception e) when (_ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw _ExceptionWrapper.Wrap(e);
		}
	}

	public Task<Container?> GetAsync(IContainerId id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		_CheckAvailability();

		bool exists;
		try
		{
			exists = Directory.Exists(_GetContainerPath(id.Concrete()));
		}
		catch (Exception e) when (_ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw _ExceptionWrapper.Wrap(e);
		}

		return Task.FromResult<Container?>(exists ? Container.IContext.RestoreContainer(id.Concrete()) : null);
	}

	public Task RemoveAsync(IContainerId id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		_CheckAvailability();

		bool exists;

		try
		{
			exists = Directory.Exists(_GetContainerPath(id.Concrete()));
			if (exists)
			{
				Directory.Delete(_GetContainerPath(id.Concrete()), true);
			}
		}
		catch (Exception e) when (_ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw _ExceptionWrapper.Wrap(e);
		}

		if (!exists)
		{
			throw new ContainerException(ContainerError.NotExists);
		}

		return Task.CompletedTask;
	}

	private static IExceptionWrapper _ExceptionWrapper { get; } = new HostContainerExceptionWrapper();

	private sealed class HostContainerExceptionWrapper : IExceptionWrapper
	{
		public bool ShouldBeWrapped(Exception exception)
		{
			Preconditions.RequiresNotNull(exception, nameof(exception));

			return exception is not ContainerException;
		}

		public Exception Wrap(Exception exception)
		{
			Preconditions.RequiresNotNull(exception, nameof(exception));

			return new ContainerException(ContainerError.Unavailable, innerException: exception);
		}
	}
}