using Common.Core.Diagnostics;
using Common.Core.Measurement;

namespace Common.Core.Data;

public static class DataSizeUnitQuantityExtensions
{
	public static TValue MeasureInBits<TValue>(
		this IQuantity<TValue, DataSizeUnit> @this,
		IMeasurer<TValue, DataSizeUnit> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return @this.MeasureIn(DataSizeUnit.Bit, measurer);
	}

	public static ulong MeasureInBits(this IQuantity<ulong, DataSizeUnit> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.MeasureInBits(DataSizeUnit.Measurers.Ulong);
	}

	public static double MeasureInBits(this IQuantity<double, DataSizeUnit> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.MeasureInBits(DataSizeUnit.Measurers.Double);
	}

	public static TValue MeasureInKilobits<TValue>(
		this IQuantity<TValue, DataSizeUnit> @this,
		IMeasurer<TValue, DataSizeUnit> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return @this.MeasureIn(DataSizeUnit.Kilobit, measurer);
	}

	public static ulong MeasureInKilobits(this IQuantity<ulong, DataSizeUnit> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.MeasureInKilobits(DataSizeUnit.Measurers.Ulong);
	}

	public static double MeasureInKilobits(this IQuantity<double, DataSizeUnit> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.MeasureInKilobits(DataSizeUnit.Measurers.Double);
	}

	public static TValue MeasureInMegabits<TValue>(
		this IQuantity<TValue, DataSizeUnit> @this,
		IMeasurer<TValue, DataSizeUnit> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return @this.MeasureIn(DataSizeUnit.Megabit, measurer);
	}

	public static ulong MeasureInMegabits(this IQuantity<ulong, DataSizeUnit> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.MeasureInMegabits(DataSizeUnit.Measurers.Ulong);
	}

	public static double MeasureInMegabits(this IQuantity<double, DataSizeUnit> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.MeasureInMegabits(DataSizeUnit.Measurers.Double);
	}

	public static TValue MeasureInBytes<TValue>(
		this IQuantity<TValue, DataSizeUnit> @this,
		IMeasurer<TValue, DataSizeUnit> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return @this.MeasureIn(DataSizeUnit.Byte, measurer);
	}

	public static ulong MeasureInBytes(this IQuantity<ulong, DataSizeUnit> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.MeasureInBytes(DataSizeUnit.Measurers.Ulong);
	}

	public static double MeasureInBytes(this IQuantity<double, DataSizeUnit> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.MeasureInBytes(DataSizeUnit.Measurers.Double);
	}

	public static TValue MeasureInKilobytes<TValue>(
		this IQuantity<TValue, DataSizeUnit> @this,
		IMeasurer<TValue, DataSizeUnit> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return @this.MeasureIn(DataSizeUnit.Kilobyte, measurer);
	}

	public static ulong MeasureInKilobytes(this IQuantity<ulong, DataSizeUnit> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.MeasureInKilobytes(DataSizeUnit.Measurers.Ulong);
	}

	public static double MeasureInKilobytes(this IQuantity<double, DataSizeUnit> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.MeasureInKilobytes(DataSizeUnit.Measurers.Double);
	}

	public static TValue MeasureInMegabytes<TValue>(
		this IQuantity<TValue, DataSizeUnit> @this,
		IMeasurer<TValue, DataSizeUnit> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return @this.MeasureIn(DataSizeUnit.Megabyte, measurer);
	}

	public static ulong MeasureInMegabytes(this IQuantity<ulong, DataSizeUnit> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.MeasureInMegabytes(DataSizeUnit.Measurers.Ulong);
	}

	public static double MeasureInMegabytes(this IQuantity<double, DataSizeUnit> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.MeasureInMegabytes(DataSizeUnit.Measurers.Double);
	}
}