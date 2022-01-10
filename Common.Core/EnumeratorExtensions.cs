using Common.Core.Diagnostics;

namespace Common.Core;

public static class EnumeratorExtensions
{
	public static T Next<T>(this IEnumerator<T> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		if (!@this.MoveNext())
		{
			throw new IndexOutOfRangeException();
		}

		return @this.Current;
	}
}