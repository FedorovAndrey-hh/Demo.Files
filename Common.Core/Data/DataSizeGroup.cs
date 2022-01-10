using Common.Core.Diagnostics;
using Common.Core.MathConcepts.GroupKind;
using Common.Core.Measurement;

namespace Common.Core.Data;

internal sealed class DataSizeGroup<TValue>
	: IGroup<DataSize<TValue>>,
	  ISemigroup<DataSize<TValue>>.IOperation,
	  IGroup<DataSize<TValue>>.IInverseOperation
	where TValue : notnull
{
	public DataSizeGroup(
		DataSize<TValue> identity,
		IGroup<TValue> valueGroup,
		IMeasurer<TValue, DataSizeUnit> measurer,
		DataSizeUnit? precision)
	{
		Preconditions.RequiresNotNull(identity, nameof(identity));
		Preconditions.RequiresNotNull(valueGroup, nameof(valueGroup));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		_identity = identity;
		_valueGroup = valueGroup;
		_measurer = measurer;
		_precision = precision;
	}

	private readonly DataSize<TValue> _identity;
	private readonly IGroup<TValue> _valueGroup;
	private readonly IMeasurer<TValue, DataSizeUnit> _measurer;
	private readonly DataSizeUnit? _precision;

	public DataSize<TValue> Apply(DataSize<TValue> left, DataSize<TValue> right)
	{
		Preconditions.RequiresNotNull(left, nameof(left));
		Preconditions.RequiresNotNull(right, nameof(right));

		var resultUnit = _precision ?? _FindBestUnit(left.Unit, right.Unit);

		return new DataSize<TValue>(
			_valueGroup.Combine(left.MeasureIn(resultUnit, _measurer), right.MeasureIn(resultUnit, _measurer)),
			resultUnit
		);
	}

	public DataSize<TValue> Apply(DataSize<TValue> element)
	{
		return new DataSize<TValue>(_valueGroup.Inverse(element.Value), element.Unit);
	}

	public ISemigroup<DataSize<TValue>>.IOperation Operation => this;
	public DataSize<TValue> Identity => _identity;
	public IGroup<DataSize<TValue>>.IInverseOperation InverseOperation => this;

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