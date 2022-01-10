using Common.Core.Diagnostics;
using Common.Core.Text;

namespace Common.Core.Measurement;

public class DecimalUnitPrefix
	: IEquatable<DecimalUnitPrefix>,
	  IComparable<DecimalUnitPrefix>,
	  IFormattable,
	  IExternalFormattable<DecimalUnitPrefix>
{
	public static NonNegative Yotta { get; } = new(24);
	public static NonNegative Zetta { get; } = new(21);
	public static NonNegative Exa { get; } = new(18);
	public static NonNegative Peta { get; } = new(15);
	public static NonNegative Tera { get; } = new(12);
	public static NonNegative Giga { get; } = new(9);
	public static NonNegative Mega { get; } = new(6);
	public static NonNegative Kilo { get; } = new(3);
	public static NonNegative Hecto { get; } = new(2);
	public static NonNegative Deca { get; } = new(1);
	public static NonNegative None { get; } = new(0);
	public static DecimalUnitPrefix Deci { get; } = new(-1);
	public static DecimalUnitPrefix Centi { get; } = new(-2);
	public static DecimalUnitPrefix Milli { get; } = new(-3);
	public static DecimalUnitPrefix Micro { get; } = new(-6);
	public static DecimalUnitPrefix Nano { get; } = new(-9);
	public static DecimalUnitPrefix Pico { get; } = new(-12);
	public static DecimalUnitPrefix Femto { get; } = new(-15);
	public static DecimalUnitPrefix Atto { get; } = new(-18);
	public static DecimalUnitPrefix Zepto { get; } = new(-21);
	public static DecimalUnitPrefix Yocto { get; } = new(-24);

	public static DecimalUnitPrefixMeasurers Measurers => DecimalUnitPrefixMeasurers.Create();

	public sealed class NonNegative : DecimalUnitPrefix
	{
		internal NonNegative(int power)
			: base(Preconditions.RequiresNonNegative(power, nameof(power)))
		{
		}
	}

	public int Power { get; }

	private DecimalUnitPrefix(int power)
	{
		Power = power;
	}

	public static bool operator ==(DecimalUnitPrefix? lhs, DecimalUnitPrefix? rhs) => Eq.ValueSafe(lhs, rhs);

	public static bool operator !=(DecimalUnitPrefix? lhs, DecimalUnitPrefix? rhs) => !Eq.ValueSafe(lhs, rhs);

	public static bool operator >(DecimalUnitPrefix lhs, DecimalUnitPrefix rhs)
	{
		Preconditions.RequiresNotNull(lhs, nameof(lhs));
		Preconditions.RequiresNotNull(rhs, nameof(rhs));

		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(DecimalUnitPrefix lhs, DecimalUnitPrefix rhs)
	{
		Preconditions.RequiresNotNull(lhs, nameof(lhs));
		Preconditions.RequiresNotNull(rhs, nameof(rhs));

		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(DecimalUnitPrefix lhs, DecimalUnitPrefix rhs)
	{
		Preconditions.RequiresNotNull(lhs, nameof(lhs));
		Preconditions.RequiresNotNull(rhs, nameof(rhs));

		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(DecimalUnitPrefix lhs, DecimalUnitPrefix rhs)
	{
		Preconditions.RequiresNotNull(lhs, nameof(lhs));
		Preconditions.RequiresNotNull(rhs, nameof(rhs));

		return lhs.CompareTo(rhs) <= 0;
	}

	public override bool Equals(object? obj) => obj is DecimalUnitPrefix prefix && Equals(prefix);

	public bool Equals(DecimalUnitPrefix? other) => other is not null && Eq.StructSafe(Power, other.Power);

	public override int GetHashCode() => Power.GetHashCode();

	public int CompareTo(DecimalUnitPrefix? other) => Power.CompareTo(other?.Power ?? 0);

	public string ToString(IExternalFormatter<DecimalUnitPrefix> formatter)
	{
		Preconditions.RequiresNotNull(formatter, nameof(formatter));

		return formatter.Format(this);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
		=> this.HandleCustomFormatters(format, formatProvider) ?? ToString();

	public override string ToString() => $"{nameof(DecimalUnitPrefix)}({nameof(Power)} = {Power})";
}