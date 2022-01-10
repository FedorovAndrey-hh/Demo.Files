using System.Text.Json.Serialization;

namespace Demo.Files.Common.Contracts.Communication;

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