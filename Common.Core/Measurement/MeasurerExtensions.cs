using Common.Core.Diagnostics;

namespace Common.Core.Measurement;

public static class MeasurerExtensions
{
	public static IEqualityComparer<IQuantity<TValue, TUnit>> GetEqualityComparer<TValue, TUnit>(
		this IMeasurer<TValue, TUnit> @this,
		TUnit unit,
		IEqualityComparer<TValue> valueEqualityComparer)
		where TValue : notnull
		where TUnit : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(unit, nameof(unit));
		Preconditions.RequiresNotNull(valueEqualityComparer, nameof(valueEqualityComparer));

		return new MeasurableEqualityComparer<TValue, TUnit>(@this, unit, valueEqualityComparer);
	}

	public static IComparer<IQuantity<TValue, TUnit>> GetComparer<TValue, TUnit>(
		this IMeasurer<TValue, TUnit> @this,
		TUnit unit,
		IComparer<TValue> valueComparer)
		where TValue : notnull
		where TUnit : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(unit, nameof(unit));
		Preconditions.RequiresNotNull(valueComparer, nameof(valueComparer));

		return new MeasuringComparer<TValue, TUnit>(@this, unit, valueComparer);
	}
}