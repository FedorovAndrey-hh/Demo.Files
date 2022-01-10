using Common.Core;
using Common.Core.Data;
using Common.Core.Diagnostics;
using Common.Core.Progressions;
using Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;
using File = Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate.File;
using TaskExtensions = Common.Core.TaskExtensions;

namespace Demo.Files.PhysicalFiles.Domain.Adapters.Context.Testing.ContainerAggregate;

public sealed class ContainerContext
	: Container.IContext,
	  IContainers
{
	private readonly DataHolder _dataHolder = new();

	private readonly IEnumerator<ContainerId> _containerIdGenerator =
		new DelegateProgression<ContainerId>(
			ContainerId.Of(0),
			id => ContainerId.Of(id.Value + 1)
		).GetEnumerator();

	private readonly IEnumerator<FileId> _fileIdGenerator =
		new DelegateProgression<FileId>(
			FileId.Of(0),
			id => FileId.Of(id.Value + 1)
		).GetEnumerator();

	Task<Container> Container.IContext.CreateAsync()
	{
		lock (_dataHolder.Lock)
		{
			return Container.IContext.RestoreContainer(_containerIdGenerator.Next()).AsTaskResult<Container>();
		}
	}

	Task<File?> Container.IContext.GetFileAsync(IContainerId containerId, IFileId id)
	{
		Preconditions.RequiresNotNull(containerId, nameof(containerId));

		lock (_dataHolder.Lock)
		{
			if (_dataHolder.Data.TryGetValue(containerId.Concrete(), out var containerData)
			    && containerData.TryGetValue(id, out var fileData))
			{
				return Container.IContext.RestoreFile(id, DataSize.Bytes((ulong)fileData.Length))
					.AsTaskResult<File?>();
			}
			else
			{
				return Task.FromResult<File?>(null);
			}
		}
	}

	Task<(IFileId, Stream)> Container.IContext.CreateFileAsync(IContainerId containerId)
	{
		Preconditions.RequiresNotNull(containerId, nameof(containerId));

		lock (_dataHolder.Lock)
		{
			var id = _fileIdGenerator.Next();
			var containerData = _dataHolder.GetOrCreateContainerData(containerId.Concrete());

			containerData[id] = DataHolder.WaitingForWrite;

			var stream = new WriteStream(containerId.Concrete(), id, _dataHolder);
			return (id, stream).AsTaskResult<(IFileId, Stream)>();
		}
	}

	Task<bool> Container.IContext.DeleteFileIfExistsAsync(IContainerId containerId, IFileId id)
	{
		Preconditions.RequiresNotNull(containerId, nameof(containerId));

		lock (_dataHolder.Lock)
		{
			if (_dataHolder.Data.TryGetValue(containerId.Concrete(), out var containerData))
			{
				return containerData.Remove(id).AsTaskResult();
			}
			else
			{
				return TaskExtensions.FalseResult;
			}
		}
	}

	Task<bool> Container.IContext.IsFileExistsAsync(IContainerId containerId, IFileId id)
	{
		Preconditions.RequiresNotNull(containerId, nameof(containerId));

		lock (_dataHolder.Lock)
		{
			return (_dataHolder.Data.TryGetValue(containerId.Concrete(), out var containerData)
				? containerData.ContainsKey(id)
				: false).AsTaskResult();
		}
	}

	Task<Stream?> Container.IContext.ReadFileAsync(IContainerId containerId, IFileId id)
	{
		Preconditions.RequiresNotNull(containerId, nameof(containerId));

		lock (_dataHolder.Lock)
		{
			if (_dataHolder.Data.TryGetValue(containerId.Concrete(), out var containerData)
			    && containerData.TryGetValue(id, out var fileData))
			{
				if (ReferenceEquals(fileData, DataHolder.WaitingForWrite))
				{
					throw new ContainerException(ContainerError.ConcurrentAccess);
				}

				return new MemoryStream(fileData).AsTaskResult<Stream?>();
			}
			else
			{
				return Task.FromResult<Stream?>(null);
			}
		}
	}

	Task<Stream?> Container.IContext.WriteFileAsync(IContainerId containerId, IFileId id)
	{
		Preconditions.RequiresNotNull(containerId, nameof(containerId));

		lock (_dataHolder.Lock)
		{
			Stream? stream =
				_dataHolder.Data.TryGetValue(containerId.Concrete(), out var containerData)
				&& containerData.ContainsKey(id)
					? new WriteStream(containerId.Concrete(), id, _dataHolder)
					: null;

			return stream.AsTaskResult();
		}
	}

	public Task<Container?> GetAsync(IContainerId id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		lock (_dataHolder.Lock)
		{
			return Task.FromResult<Container?>(
				_dataHolder.Data.ContainsKey(id.Concrete())
					? Container.IContext.RestoreContainer(id.Concrete())
					: null
			);
		}
	}

	public Task RemoveAsync(IContainerId id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		lock (_dataHolder.Lock)
		{
			if (!_dataHolder.Data.Remove(id.Concrete()))
			{
				throw new ContainerException(ContainerError.NotExists);
			}

			return Task.CompletedTask;
		}
	}

	private sealed class DataHolder
	{
		public static byte[] WaitingForWrite { get; } = new byte[0];

		public object Lock { get; } = new();

		public IDictionary<ContainerId, IDictionary<IFileId, byte[]>> Data { get; } =
			new Dictionary<ContainerId, IDictionary<IFileId, byte[]>>();

		public IDictionary<IFileId, byte[]> GetOrCreateContainerData(ContainerId id)
		{
			if (!Data.TryGetValue(id, out var result))
			{
				Data[id] = result = new Dictionary<IFileId, byte[]>();
			}

			return result;
		}
	}

	private sealed class WriteStream : MemoryStream
	{
		public WriteStream(ContainerId containerId, IFileId fileId, DataHolder dataHolder)
		{
			Preconditions.RequiresNotNull(dataHolder, nameof(dataHolder));

			_containerId = containerId;
			_fileId = fileId;
			_dataHolder = dataHolder;
		}

		private readonly ContainerId _containerId;
		private readonly IFileId _fileId;
		private readonly DataHolder _dataHolder;

		public override void Flush()
		{
			base.Flush();

			lock (_dataHolder.Lock)
			{
				if (_dataHolder.Data.TryGetValue(_containerId, out var containerData))
				{
					if (ReferenceEquals(containerData[_fileId], DataHolder.WaitingForWrite))
					{
						containerData[_fileId] = ToArray();
					}
				}
			}
		}
	}
}