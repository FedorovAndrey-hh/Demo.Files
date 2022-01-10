using Common.Core.Diagnostics;
using Common.Core.MathConcepts.GroupKind;
using Common.Core.Measurement;

namespace Common.Core.Data;

internal sealed class DataSizeMonoid<TValue>
	: IMonoid<DataSize<TValue>>,
	  ISemigroup<DataSize<TValue>>.IOperation
	where TValue : notnull
{
	public DataSizeMonoid(
		DataSize<TValue> identity,
		IMonoid<TValue> valueMonoid,
		IMeasurer<TValue, DataSizeUnit> measurer,
		DataSizeUnit? precision)
	{
		Preconditions.RequiresNotNull(identity, nameof(identity));
		Preconditions.RequiresNotNull(valueMonoid, nameof(valueMonoid));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		_identity = identity;
		_valueMonoid = valueMonoid;
		_measurer = measurer;
		_precision = precision;
	}

	private readonly DataSize<TValue> _identity;
	private readonly IMonoid<TValue> _valueMonoid;
	private readonly IMeasurer<TValue, DataSizeUnit> _measurer;
	private readonly DataSizeUnit? _precision;

	public DataSize<TValue> Apply(DataSize<TValue> left, DataSize<TValue> right)
	{
		Preconditions.RequiresNotNull(left, nameof(left));
		Preconditions.RequiresNotNull(right, nameof(right));

		var resultUnit = _precision ?? _FindBestUnit(left.Unit, right.Unit);

		return new DataSize<TValue>(
			_valueMonoid.Combine(left.MeasureIn(resultUnit, _measurer), right.MeasureIn(resultUnit, _measurer)),
			resultUnit
		);
	}

	public ISemigroup<DataSize<TValue>>.IOperation Operation => this;
	public DataSize<TValue> Identity => _identity;

	private static DataSizeUnit _FindBestUnit(DataSizeUnit unit1, DataSizeUnit unit2)
	{
		Preconditions.RequiresNotNull(unit1, nameof(unit1));
		Preconditions.RequiresNotNull(unit2, nameof(unit2));

		return DataSizeUnit.Custom(
			Math.Min(unit1.BitCount, unit2.BitCount),
			MathExtensions.Min(unit1.UnitPrefix, unit2.UnitPrefix)
		);
	}
}