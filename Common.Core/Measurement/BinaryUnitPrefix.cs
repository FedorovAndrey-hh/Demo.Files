using Common.Core.Diagnostics;
using Common.Core.Text;

namespace Common.Core.Measurement;

public sealed class BinaryUnitPrefix
	: IEquatable<BinaryUnitPrefix>,
	  IComparable<BinaryUnitPrefix>,
	  IFormattable,
	  IExternalFormattable<BinaryUnitPrefix>
{
	public static BinaryUnitPrefix Yobi { get; } = new(8);
	public static BinaryUnitPrefix Zebi { get; } = new(7);
	public static BinaryUnitPrefix Exbi { get; } = new(6);
	public static BinaryUnitPrefix Pebi { get; } = new(5);
	public static BinaryUnitPrefix Tebi { get; } = new(4);
	public static BinaryUnitPrefix Gibi { get; } = new(3);
	public static BinaryUnitPrefix Mebi { get; } = new(2);
	public static BinaryUnitPrefix Kibi { get; } = new(1);
	public static BinaryUnitPrefix None { get; } = new(0);

	public int Power { get; }

	private BinaryUnitPrefix(int power)
	{
		Power = power;
	}

	public static bool operator ==(BinaryUnitPrefix? lhs, BinaryUnitPrefix? rhs) => Eq.ValueSafe(lhs, rhs);

	public static bool operator !=(BinaryUnitPrefix? lhs, BinaryUnitPrefix? rhs) => !Eq.ValueSafe(lhs, rhs);

	public static bool operator >(BinaryUnitPrefix lhs, BinaryUnitPrefix rhs)
	{
		Preconditions.RequiresNotNull(lhs, nameof(lhs));
		Preconditions.RequiresNotNull(rhs, nameof(rhs));

		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(BinaryUnitPrefix lhs, BinaryUnitPrefix rhs)
	{
		Preconditions.RequiresNotNull(lhs, nameof(lhs));
		Preconditions.RequiresNotNull(rhs, nameof(rhs));

		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(BinaryUnitPrefix lhs, BinaryUnitPrefix rhs)
	{
		Preconditions.RequiresNotNull(lhs, nameof(lhs));
		Preconditions.RequiresNotNull(rhs, nameof(rhs));

		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(BinaryUnitPrefix lhs, BinaryUnitPrefix rhs)
	{
		Preconditions.RequiresNotNull(lhs, nameof(lhs));
		Preconditions.RequiresNotNull(rhs, nameof(rhs));

		return lhs.CompareTo(rhs) <= 0;
	}

	public override bool Equals(object? obj) => obj is BinaryUnitPrefix prefix && Equals(prefix);

	public bool Equals(BinaryUnitPrefix? other) => other is not null && Eq.StructSafe(Power, other.Power);

	public override int GetHashCode() => Power.GetHashCode();

	public int CompareTo(BinaryUnitPrefix? other) => Power.CompareTo(other?.Power ?? 0);

	public string ToString(IExternalFormatter<BinaryUnitPrefix> formatter)
	{
		Preconditions.RequiresNotNull(formatter, nameof(formatter));

		return formatter.Format(this);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
		=> this.HandleCustomFormatters(format, formatProvider) ?? ToString();

	public override string ToString() => $"{nameof(BinaryUnitPrefix)}({nameof(Power)} = {Power})";
}