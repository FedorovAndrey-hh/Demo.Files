using Common.Core.Diagnostics;

namespace Common.Core.Measurement;

public static class QuantityExtensions
{
	public static TValue MeasureIn<TValue, TUnit>(
		this IQuantity<TValue, TUnit> @this,
		TUnit unit,
		IMeasurer<TValue, TUnit> measurer)
		where TValue : notnull
		where TUnit : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(unit, nameof(unit));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(@this.Value, @this.Unit, unit);
	}
}