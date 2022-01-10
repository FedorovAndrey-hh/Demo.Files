using System.Diagnostics.CodeAnalysis;
using Common.Core.Diagnostics;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.Testing.StorageAggregate;

public static class StorageIdExtensions
{
	[return: NotNullIfNotNull("this")]
	public static StorageId? Concrete(this IStorageId? @this) => (StorageId?)@this;

	public static long RawLong(this IStorageId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Concrete().Value;
	}
}