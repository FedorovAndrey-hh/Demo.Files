using Common.Core.Diagnostics;

namespace Common.Core;

public static class TypeExtensions
{
	public static bool Equals<T>(this Type @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return Eq.Value(@this, typeof(T));
	}

	public static bool IsSubclassOf<T>(this Type @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.IsSubclassOf(typeof(T));
	}
}