using Common.Core.Diagnostics;

namespace Common.Core.Measurement;

public sealed class MeasurableEqualityComparer<TValue, TUnit> : IEqualityComparer<IQuantity<TValue, TUnit>>
	where TValue : notnull
	where TUnit : notnull
{
	public MeasurableEqualityComparer(
		IMeasurer<TValue, TUnit> measurer,
		TUnit measuringUnit,
		IEqualityComparer<TValue> valueComparer)
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
	private readonly IEqualityComparer<TValue> _valueComparer;

	public bool Equals(IQuantity<TValue, TUnit>? x, IQuantity<TValue, TUnit>? y)
	{
		if (ReferenceEquals(x, y))
		{
			return true;
		}

		if (x is null || y is null)
		{
			return false;
		}

		return _valueComparer.Equals(
			_measurer.Measure(x.Value, x.Unit, _measuringUnit),
			_measurer.Measure(y.Value, y.Unit, _measuringUnit)
		);
	}

	public int GetHashCode(IQuantity<TValue, TUnit> obj)
	{
		Preconditions.RequiresNotNull(obj, nameof(obj));

		return _valueComparer.GetHashCode(_measurer.Measure(obj.Value, obj.Unit, _measuringUnit));
	}
}