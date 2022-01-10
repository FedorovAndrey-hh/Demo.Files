using Common.Core.Diagnostics;

namespace Common.Core.Measurement;

public sealed class MeasuringComparer<TValue, TUnit> : IComparer<IQuantity<TValue, TUnit>>
	where TValue : notnull
	where TUnit : notnull
{
	public MeasuringComparer(
		IMeasurer<TValue, TUnit> measurer,
		TUnit measuringUnit,
		IComparer<TValue> valueComparer)
	{
		Preconditions.RequiresNotNull(measurer, nameof(measurer));
		Preconditions.RequiresNotNull(measuringUnit, nameof(measuringUnit));
		Preconditions.RequiresNotNull(valueComparer, nameof(valueComparer));

		_measurer = measurer;
		_measuringUnit = measuringUnit;
		_valueComparer = valueComparer;
	}

	private readonly IMeasurer<TValue, TUnit> _measurer;
	private readonly TUnit _measuringUnit;
	private readonly IComparer<TValue> _valueComparer;

	public int Compare(IQuantity<TValue, TUnit>? x, IQuantity<TValue, TUnit>? y)
		=> ComparerUtility.CompareReferencesNullFirst(x, y, out var result)
			? result
			: _valueComparer.Compare(
				_measurer.Measure(x.Value, x.Unit, _measuringUnit),
				_measurer.Measure(y.Value, y.Unit, _measuringUnit)
			);
}