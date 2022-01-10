using System.Diagnostics.CodeAnalysis;
using Common.Core.Diagnostics;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

public static class FileIdExtensions
{
	[return: NotNullIfNotNull("this")]
	public static FileId? Concrete(this IFileId? @this) => (FileId?)@this;

	public static long RawLong(this IFileId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Concrete().Value;
	}
}