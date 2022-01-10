using Common.Core.Diagnostics;
using Common.Core.Measurement;
using Common.Core.Text;

namespace Common.Core.Data;

public static class DataSize
{
	public static DataSize<TValue> Zero<TValue>()
		where TValue : struct
		=> new(default, DataSizeUnit.Bit);

	public static DataSize<TValue> Bits<TValue>(TValue value, DecimalUnitPrefix.NonNegative unitPrefix)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(unitPrefix, nameof(unitPrefix));
		Preconditions.RequiresNotNull(value, nameof(value));

		return new DataSize<TValue>(value, DataSizeUnit.Bits(unitPrefix));
	}

	public static DataSize<TValue> Bits<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Bit);

	public static DataSize<TValue> Kilobits<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Kilobit);

	public static DataSize<TValue> Megabits<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Megabit);

	public static DataSize<TValue> Gigabits<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Gigabit);

	public static DataSize<TValue> Terabits<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Terabit);

	public static DataSize<TValue> Petabits<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Petabit);

	public static DataSize<TValue> Exabits<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Exabit);

	public static DataSize<TValue> Zettabits<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Zettabit);

	public static DataSize<TValue> Yottabits<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Yottabit);

	public static DataSize<TValue> Bytes<TValue>(TValue value, DecimalUnitPrefix.NonNegative unitPrefix)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(unitPrefix, nameof(unitPrefix));

		return new DataSize<TValue>(value, DataSizeUnit.Bytes(unitPrefix));
	}

	public static DataSize<TValue> Bytes<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Byte);

	public static DataSize<TValue> Kilobytes<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Kilobyte);

	public static DataSize<TValue> Megabytes<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Megabyte);

	public static DataSize<TValue> Gigabytes<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Gigabyte);

	public static DataSize<TValue> Terabytes<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Terabyte);

	public static DataSize<TValue> Petabytes<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Petabyte);

	public static DataSize<TValue> Exabytes<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Exabyte);

	public static DataSize<TValue> Zettabytes<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Zettabyte);

	public static DataSize<TValue> Yottabytes<TValue>(TValue value)
		where TValue : notnull
		=> new(value, DataSizeUnit.Yottabyte);

	public static DataSize<TValue> Of<TValue>(TValue value, DataSizeUnit unit)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(unit, nameof(unit));

		return new DataSize<TValue>(value, unit);
	}

	public static DataSizeMonoids Monoids => DataSizeMonoids.Create();
	public static DataSizeGroups Groups => DataSizeGroups.Create();
	public static DataSizeLinearSpaces LinearSpaces => DataSizeLinearSpaces.Create();

	public static DataSizeUnitQuantityEqualityComparers EqualityComparers
		=> DataSizeUnitQuantityEqualityComparers.Create();

	public static DataSizeUnitQuantityComparers Comparers => DataSizeUnitQuantityComparers.Create();
}

public sealed class DataSize<TValue>
	: IQuantity<TValue, DataSizeUnit>,
	  IFormattable,
	  IExternalFormattable<DataSize<TValue>>
	where TValue : notnull
{
	public DataSize(TValue value, DataSizeUnit unit)
	{
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(unit, nameof(unit));

		Value = value;
		Unit = unit;
	}

	public TValue Value { get; }
	public DataSizeUnit Unit { get; }

	public string ToString(IExternalFormatter<DataSize<TValue>> formatter)
	{
		Preconditions.RequiresNotNull(formatter, nameof(formatter));

		return formatter.Format(this);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
		=> this.HandleCustomFormatters(format, formatProvider) ?? ToString();

	public override string ToString() => $"{nameof(DataSize<TValue>)}({Value} {Unit})";
}