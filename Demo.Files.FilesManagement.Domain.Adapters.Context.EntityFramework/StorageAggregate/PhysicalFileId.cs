using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

public sealed record PhysicalFileId : IPhysicalFileId
{
	public static PhysicalFileId Of(Guid value) => new(value);

	private PhysicalFileId(Guid value) => Value = value;

	public Guid Value { get; }

	public bool Equals(IPhysicalFileId? other) => other is PhysicalFileId typedOther && Equals(typedOther);
}