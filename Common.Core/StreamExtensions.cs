using Common.Core.Data;
using Common.Core.Diagnostics;

namespace Common.Core;

public static class StreamExtensions
{
	public static DataSize<long> GetSize(this Stream @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return DataSize.Bytes(@this.Length);
	}

	public static DataSize<ulong> GetSizeUnsigned(this Stream @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return DataSize.Bytes((ulong)@this.Length);
	}

	public static DataSize<double> GetSizeInDouble(this Stream @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return DataSize.Bytes((double)@this.Length);
	}
}