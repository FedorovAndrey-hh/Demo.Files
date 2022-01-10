using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

[Table("Files")]
public class FileData
{
	[Key]
	[Column("id")]
	public long Id { get; set; }

	[Column("physicalId")]
	public Guid PhysicalId { get; set; }

	[Column("name")]
	public string Name { get; set; } = null!;

	[Column("size")]
	public ulong Size { get; set; }

	[Column("directoryId")]
	public long DirectoryId { get; set; }
}