using Common.Core;
using Common.Core.Data;
using Common.Core.Diagnostics;

namespace Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

public sealed class Limitations : IEquatable<Limitations>
{
	public static Limitations Maximum { get; } = new(
		DataSize.Zero<ulong>(),
		0,
		DataSize.Zero<ulong>()
	);

	public static Limitations Minimum { get; } = new(
		10ul.Gigabytes(),
		1000,
		500ul.Megabytes()
	);

	public static Limitations Create(
		DataSize<ulong> totalSpace,
		uint totalFileCount,
		DataSize<ulong> singleFileSize)
	{
		Preconditions.RequiresNotNull(totalSpace, nameof(totalSpace));
		Preconditions.RequiresNotNull(singleFileSize, nameof(singleFileSize));

		if (totalSpace.GraterThan(Minimum.TotalSpace, Storage.DataSizeComparer))
		{
			throw new StorageException(StorageError.LimitationsTotalSpaceTooLarge);
		}

		if (totalFileCount > Minimum.TotalFileCount)
		{
			throw new StorageException(StorageError.LimitationsTotalFileCountTooLarge);
		}

		if (singleFileSize.GraterThan(Minimum.SingleFileSize, Storage.DataSizeComparer))
		{
			throw new StorageException(StorageError.LimitationsTotalSingleFileSizeTooLarge);
		}

		return new Limitations(totalSpace, totalFileCount, singleFileSize);
	}

	private Limitations(DataSize<ulong> totalSpace, uint totalFileCount, DataSize<ulong> singleFileSize)
	{
		Preconditions.RequiresNotNull(totalSpace, nameof(totalSpace));
		Preconditions.RequiresNotNull(singleFileSize, nameof(singleFileSize));

		TotalSpace = totalSpace;
		TotalFileCount = totalFileCount;
		SingleFileSize = singleFileSize;
	}

	public DataSize<ulong> TotalSpace { get; }
	public uint TotalFileCount { get; }
	public DataSize<ulong> SingleFileSize { get; }

	public static bool operator ==(Limitations? lhs, Limitations? rhs) => Eq.ValueSafe(lhs, rhs);
	public static bool operator !=(Limitations? lhs, Limitations? rhs) => !Eq.ValueSafe(lhs, rhs);

	public bool Equals(Limitations? other)
		=> other is not null
		   && Storage.DataSizeEqualityComparer.Equals(TotalSpace, other.TotalSpace)
		   && Eq.ValueSafe(TotalFileCount, other.TotalFileCount)
		   && Storage.DataSizeEqualityComparer.Equals(SingleFileSize, other.SingleFileSize);

	public override bool Equals(object? obj) => obj is Limitations other && Equals(other);

	public override int GetHashCode()
		=> HashCode.Combine(
			Storage.DataSizeEqualityComparer.GetHashCode(TotalSpace),
			TotalFileCount,
			Storage.DataSizeEqualityComparer.GetHashCode(SingleFileSize)
		);
}