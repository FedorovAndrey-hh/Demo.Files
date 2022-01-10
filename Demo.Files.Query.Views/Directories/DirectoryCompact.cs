using System.Text.Json.Serialization;
using Common.Core.Diagnostics;

namespace Demo.Files.Query.Views.Directories;

public sealed class DirectoryCompact
{
	[JsonConstructor]
	public DirectoryCompact(
		long id,
		string name,
		long sizeInBytes,
		long filesCount,
		long storageId)
	{
		Preconditions.RequiresNotNull(name, nameof(name));

		Id = id;
		Name = name;
		SizeInBytes = sizeInBytes;
		FilesCount = filesCount;
		StorageId = storageId;
	}

	public long Id { get; set; }

	public string Name { get; set; }

	public long SizeInBytes { get; set; }

	public long FilesCount { get; set; }

	public long StorageId { get; set; }
}