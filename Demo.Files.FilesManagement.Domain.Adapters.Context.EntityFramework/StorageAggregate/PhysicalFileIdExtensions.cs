using System.Diagnostics.CodeAnalysis;
using Common.Core.Diagnostics;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

public static class PhysicalFileIdExtensions
{
	[return: NotNullIfNotNull("this")]
	public static PhysicalFileId? Concrete(this IPhysicalFileId? @this) => (PhysicalFileId?)@this;

	public static Guid RawGuid(this IPhysicalFileId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Concrete().Value;
	}
}