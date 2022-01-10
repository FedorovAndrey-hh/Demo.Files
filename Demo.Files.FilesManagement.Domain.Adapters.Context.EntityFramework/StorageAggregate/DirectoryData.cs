using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

[Table("Directories")]
public class DirectoryData
{
	[Key]
	[Column("id")]
	public long Id { get; set; }

	[Column("name")]
	public string Name { get; set; } = null!;

	[Column("storageId")]
	public long StorageId { get; set; }

	public List<FileData> Files { get; set; } = null!;
}