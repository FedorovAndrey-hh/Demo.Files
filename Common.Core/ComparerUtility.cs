using System.Diagnostics.CodeAnalysis;

namespace Common.Core;

public static class ComparerUtility
{
	public static bool CompareReferencesNullFirst(
		[NotNullWhen(false)] object? lhs,
		[NotNullWhen(false)] object? rhs,
		out int result)
	{
		if (ReferenceEquals(lhs, rhs))
		{
			result = 0;
			return true;
		}

		if (lhs is null)
		{
			result = -1;
			return true;
		}

		if (rhs is null)
		{
			result = 1;
			return true;
		}

		result = 0;
		return false;
	}

	public static bool CompareReferencesNullLast(
		[NotNullWhen(false)] object? lhs,
		[NotNullWhen(false)] object? rhs,
		out int result)
	{
		if (ReferenceEquals(lhs, rhs))
		{
			result = 0;
			return true;
		}

		if (lhs is null)
		{
			result = 1;
			return true;
		}

		if (rhs is null)
		{
			result = -1;
			return true;
		}

		result = 0;
		return false;
	}
}