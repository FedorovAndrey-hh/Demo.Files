using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

[Table("Storages")]
[Index(nameof(Version))]
public class StorageData
{
	[Key]
	[Column("id")]
	public long Id { get; set; }

	[ConcurrencyCheck]
	[Column("version")]
	public ulong Version { get; set; }

	[Column("limitations_totalSpace")]
	public ulong LimitationsTotalSpace { get; set; }

	[Column("limitations_totalFileCount")]
	public uint LimitationsTotalFileCount { get; set; }

	[Column("limitations_singleFileSize")]
	public ulong LimitationsSingleFileSize { get; set; }

	public List<DirectoryData> Directories { get; set; } = null!;
}