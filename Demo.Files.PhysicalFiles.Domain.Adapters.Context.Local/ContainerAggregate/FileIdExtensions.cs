using System.Diagnostics.CodeAnalysis;
using Common.Core.Diagnostics;
using Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;

namespace Demo.Files.PhysicalFiles.Domain.Adapters.Context.Local.ContainerAggregate;

public static class FileIdExtensions
{
	[return: NotNullIfNotNull("this")]
	public static FileId? Concrete(this IFileId? @this) => (FileId?)@this;

	public static Guid RawGuid(this IFileId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Concrete().Value;
	}
}