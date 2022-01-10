namespace Common.Core;

public static class Eq
{
	public static bool Identity(object? lhs, object? rhs) => ReferenceEquals(lhs, rhs);

	public static bool Value<T>(T? lhs, T? rhs)
	{
		if (ReferenceEquals(lhs, rhs))
		{
			return true;
		}

		if (lhs is null || rhs is null)
		{
			return false;
		}

		return lhs.Equals(rhs);
	}

	public static bool ValueSafe<T>(T? lhs, T? rhs)
		where T : IEquatable<T>
	{
		if (ReferenceEquals(lhs, rhs))
		{
			return true;
		}

		if (lhs is null || rhs is null)
		{
			return false;
		}

		return lhs.Equals(rhs);
	}

	public static bool Struct<T>(T lhs, T rhs)
		where T : struct
		=> lhs.Equals(rhs);

	public static bool StructSafe<T>(T lhs, T rhs)
		where T : struct, IEquatable<T>
		=> lhs.Equals(rhs);
}