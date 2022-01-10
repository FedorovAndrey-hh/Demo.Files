using Common.Core.Data;
using Common.Core.Diagnostics;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

public static class FileDataExtensions
{
	public static void ResetId(this FileData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Id = default;
	}

	public static void SetId(this FileData @this, FileId id)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Id = id.Value;
	}

	public static FileId GetId(this FileData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return FileId.Of(@this.Id);
	}

	public static void SetPhysicalId(this FileData @this, PhysicalFileId id)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.PhysicalId = id.Value;
	}

	public static PhysicalFileId GetPhysicalId(this FileData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return PhysicalFileId.Of(@this.PhysicalId);
	}

	public static void SetName(this FileData @this, FileName name)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(name, nameof(name));

		@this.Name = name.AsString();
	}

	public static FileName GetName(this FileData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return FileName.Create(@this.Name);
	}

	public static void SetDirectoryId(this FileData @this, DirectoryId id)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.DirectoryId = id.Value;
	}

	public static DirectoryId GetDirectoryId(this FileData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return DirectoryId.Of(@this.DirectoryId);
	}

	public static void SetSize(this FileData @this, DataSize<ulong> size)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Size = size.MeasureInBytes();
	}

	public static DataSize<ulong> GetSize(this FileData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return DataSize.Bytes(@this.Size);
	}
}