using System.Diagnostics.CodeAnalysis;
using Common.Core.Diagnostics;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.Testing.StorageAggregate;

public static class DirectoryIdExtensions
{
	[return: NotNullIfNotNull("this")]
	public static DirectoryId? Concrete(this IDirectoryId? @this) => (DirectoryId?)@this;

	public static long RawLong(this IDirectoryId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Concrete().Value;
	}
}