using Common.Core.Diagnostics;
using Common.Core.Measurement;
using Common.Core.Text;
using JetBrains.Annotations;

namespace Common.Core.Data;

public sealed class DataSizeUnit
	: IFormattable,
	  IExternalFormattable<DataSizeUnit>,
	  IEquatable<DataSizeUnit>
{
	public static DataSizeUnit Bits(DecimalUnitPrefix.NonNegative unitPrefix)
	{
		Preconditions.RequiresNotNull(unitPrefix, nameof(unitPrefix));

		return new DataSizeUnit(1, unitPrefix);
	}

	public static DataSizeUnit Bit { get; } = Bits(DecimalUnitPrefix.None);
	public static DataSizeUnit Kilobit { get; } = Bits(DecimalUnitPrefix.Kilo);
	public static DataSizeUnit Megabit { get; } = Bits(DecimalUnitPrefix.Mega);
	public static DataSizeUnit Gigabit { get; } = Bits(DecimalUnitPrefix.Giga);
	public static DataSizeUnit Terabit { get; } = Bits(DecimalUnitPrefix.Tera);
	public static DataSizeUnit Petabit { get; } = Bits(DecimalUnitPrefix.Peta);
	public static DataSizeUnit Exabit { get; } = Bits(DecimalUnitPrefix.Exa);
	public static DataSizeUnit Zettabit { get; } = Bits(DecimalUnitPrefix.Zetta);
	public static DataSizeUnit Yottabit { get; } = Bits(DecimalUnitPrefix.Yotta);

	public static DataSizeUnit Bytes(DecimalUnitPrefix.NonNegative unitPrefix)
	{
		Preconditions.RequiresNotNull(unitPrefix, nameof(unitPrefix));

		return new DataSizeUnit(8, unitPrefix);
	}

	public static DataSizeUnit Byte { get; } = Bytes(DecimalUnitPrefix.None);
	public static DataSizeUnit Kilobyte { get; } = Bytes(DecimalUnitPrefix.Kilo);
	public static DataSizeUnit Megabyte { get; } = Bytes(DecimalUnitPrefix.Mega);
	public static DataSizeUnit Gigabyte { get; } = Bytes(DecimalUnitPrefix.Giga);
	public static DataSizeUnit Terabyte { get; } = Bytes(DecimalUnitPrefix.Tera);
	public static DataSizeUnit Petabyte { get; } = Bytes(DecimalUnitPrefix.Peta);
	public static DataSizeUnit Exabyte { get; } = Bytes(DecimalUnitPrefix.Exa);
	public static DataSizeUnit Zettabyte { get; } = Bytes(DecimalUnitPrefix.Zetta);
	public static DataSizeUnit Yottabyte { get; } = Bytes(DecimalUnitPrefix.Yotta);

	public static DataSizeUnit Custom(uint bitCount, DecimalUnitPrefix.NonNegative unitPrefix)
	{
		Preconditions.RequiresNotNull(unitPrefix, nameof(unitPrefix));

		return new DataSizeUnit(bitCount, unitPrefix);
	}

	[ContractAnnotation("null => null")]
	public static DataSizeUnit? ByName(string? name)
		=> name switch
		{
			nameof(Bit) => Bit,
			nameof(Kilobit) => Kilobit,
			nameof(Megabit) => Megabit,
			nameof(Gigabit) => Gigabit,
			nameof(Terabit) => Terabit,
			nameof(Petabit) => Petabit,
			nameof(Exabit) => Exabit,
			nameof(Zettabit) => Zettabit,
			nameof(Yottabit) => Yottabit,

			nameof(Byte) => Byte,
			nameof(Kilobyte) => Kilobyte,
			nameof(Megabyte) => Megabyte,
			nameof(Gigabyte) => Gigabyte,
			nameof(Terabyte) => Terabyte,
			nameof(Petabyte) => Petabyte,
			nameof(Exabyte) => Exabyte,
			nameof(Zettabyte) => Zettabyte,
			nameof(Yottabyte) => Yottabyte,

			_ => null
		};

	public static DataSizeUnitMeasurers Measurers => DataSizeUnitMeasurers.Create();

	private DataSizeUnit(uint bitCount, DecimalUnitPrefix.NonNegative unitPrefix)
	{
		Preconditions.RequiresNotNull(unitPrefix, nameof(unitPrefix));

		BitCount = bitCount;
		UnitPrefix = unitPrefix;
	}

	public uint BitCount { get; }

	public DecimalUnitPrefix.NonNegative UnitPrefix { get; }

	public static bool operator ==(DataSizeUnit? lhs, DataSizeUnit? rhs) => Eq.ValueSafe(lhs, rhs);

	public static bool operator !=(DataSizeUnit? lhs, DataSizeUnit? rhs) => !Eq.ValueSafe(lhs, rhs);

	public bool Equals(DataSizeUnit? other)
		=> ReferenceEquals(this, other)
		   || (other is not null
		       && BitCount == other.BitCount
		       && Eq.ValueSafe<DecimalUnitPrefix>(UnitPrefix, other.UnitPrefix));

	public override bool Equals(object? obj)
		=> ReferenceEquals(obj, this) || (obj is DataSizeUnit other && Equals(other));

	public override int GetHashCode() => HashCode.Combine(BitCount, UnitPrefix);

	public string ToString(IExternalFormatter<DataSizeUnit> formatter)
	{
		Preconditions.RequiresNotNull(formatter, nameof(formatter));

		return formatter.Format(this);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
		=> this.HandleCustomFormatters(format, formatProvider) ?? ToString();

	public override string ToString()
		=> $"{nameof(DataSizeUnit)}({nameof(BitCount)} = {BitCount}, {nameof(UnitPrefix)} = {UnitPrefix})";
}