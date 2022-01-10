using Common.Core.Diagnostics;

namespace Common.Core;

public static class MathExtensions
{
	public static T Max<T>(T lhs, T rhs)
		where T : IComparable<T>
	{
		Preconditions.RequiresNotNull(lhs, nameof(lhs));
		Preconditions.RequiresNotNull(rhs, nameof(rhs));

		return lhs.CompareTo(rhs) switch
		{
			> 0 => lhs,
			< 0 => rhs,
			_ => lhs
		};
	}

	public static T Min<T>(T lhs, T rhs)
		where T : IComparable<T>
	{
		Preconditions.RequiresNotNull(lhs, nameof(lhs));
		Preconditions.RequiresNotNull(rhs, nameof(rhs));

		return lhs.CompareTo(rhs) switch
		{
			> 0 => rhs,
			< 0 => lhs,
			_ => lhs
		};
	}

	private static ulong[] _powerOf10 = new[]
	{
		1ul,
		10ul,
		100ul,
		1000ul,
		10000ul,
		100000ul,
		1000000ul,
		10000000ul,
		100000000ul,
		1000000000ul,
		10000000000ul,
		100000000000ul,
		1000000000000ul,
		10000000000000ul,
		100000000000000ul,
		1000000000000000ul,
		10000000000000000ul,
		100000000000000000ul,
		1000000000000000000ul,
		10000000000000000000ul,
	};

	public static ulong PowOf10Ulong(uint power)
	{
		if (power >= _powerOf10.Length)
		{
			throw new OverflowException();
		}

		return _powerOf10[power];
	}
}