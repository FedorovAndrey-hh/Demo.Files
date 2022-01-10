using System.Text.Json.Serialization;
using Common.Core.Diagnostics;

namespace Demo.Files.Query.Views.Storages;

public sealed class StorageCompact
{
	[JsonConstructor]
	public StorageCompact(
		long id,
		ulong sizeInBytes,
		ulong directoriesCount,
		ulong filesCount,
		StorageLimitations limitations,
		ulong version)
	{
		Preconditions.RequiresNotNull(limitations, nameof(limitations));

		Id = id;
		SizeInBytes = sizeInBytes;
		DirectoriesCount = directoriesCount;
		FilesCount = filesCount;
		Limitations = limitations;
		Version = version;
	}

	public long Id { get; set; }

	public ulong SizeInBytes { get; set; }

	public ulong DirectoriesCount { get; set; }

	public ulong FilesCount { get; set; }

	public StorageLimitations Limitations { get; set; }

	public ulong Version { get; set; }

	public sealed class StorageLimitations
	{
		[JsonConstructor]
		public StorageLimitations(ulong totalSpaceInBytes, uint totalFileCount, ulong singleFileSizeInBytes)
		{
			TotalSpaceInBytes = totalSpaceInBytes;
			TotalFileCount = totalFileCount;
			SingleFileSizeInBytes = singleFileSizeInBytes;
		}

		public ulong TotalSpaceInBytes { get; set; }
		public uint TotalFileCount { get; set; }
		public ulong SingleFileSizeInBytes { get; set; }
	}
}