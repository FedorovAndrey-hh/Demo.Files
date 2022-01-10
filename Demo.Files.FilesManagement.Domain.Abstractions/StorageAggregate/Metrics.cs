using Common.Core;
using Common.Core.Data;
using Common.Core.Diagnostics;
using Common.Core.MathConcepts;
using Common.Core.MathConcepts.GroupKind;

namespace Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

public sealed class Metrics : IEquatable<Metrics>
{
	public static IMonoid<Metrics> Monoid { get; } = new DefaultMonoid();

	public static Metrics Empty { get; } = new(DataSize.Zero<ulong>(), 0);

	public static Metrics OfSingleFile(DataSize<ulong> size) => new(size, 1);

	public static Metrics OfFiles(IEnumerable<DataSize<ulong>> fileSizes)
	{
		Preconditions.RequiresNotNull(fileSizes, nameof(fileSizes));

		var totalSize = Storage.DataSizeMonoid.Identity;
		var totalCount = 0u;
		checked
		{
			foreach (var fileSize in fileSizes)
			{
				totalSize = totalSize.Combine(fileSize, Storage.DataSizeMonoid);
				totalCount++;
			}
		}

		return new Metrics(totalSize, totalCount);
	}

	private Metrics(DataSize<ulong> size, uint filesCount)
	{
		Preconditions.RequiresNotNull(size, nameof(size));

		Size = size;
		FilesCount = filesCount;
	}

	public DataSize<ulong> Size { get; }
	public uint FilesCount { get; }

	public static bool operator ==(Metrics? lhs, Metrics? rhs) => Eq.ValueSafe(lhs, rhs);
	public static bool operator !=(Metrics? lhs, Metrics? rhs) => !Eq.ValueSafe(lhs, rhs);

	public bool Equals(Metrics? other)
		=> other is not null
		   && Storage.DataSizeEqualityComparer.Equals(Size, other.Size)
		   && Eq.StructSafe(FilesCount, other.FilesCount);

	public override bool Equals(object? obj) => obj is Metrics other && Equals(other);

	public override int GetHashCode()
		=> HashCode.Combine(Storage.DataSizeEqualityComparer.GetHashCode(Size), FilesCount);

	private sealed class DefaultMonoid
		: IMonoid<Metrics>,
		  ISemigroup<Metrics>.IOperation
	{
		public Metrics Apply(Metrics left, Metrics right)
			=> new(
				left.Size.Combine(right.Size, Storage.DataSizeMonoid),
				checked(left.FilesCount + right.FilesCount)
			);

		public ISemigroup<Metrics>.IOperation Operation => this;
		public Metrics Identity => Empty;
	}
}