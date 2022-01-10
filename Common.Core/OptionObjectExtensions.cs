using Common.Core.Diagnostics;

namespace Common.Core;

public static class OptionObjectExtensions
{
	public static Option<T> AsSome<T>(this T @this)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return Option.Some(@this);
	}
}