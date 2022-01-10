using Common.Core.Diagnostics;

namespace Common.Core.MathConcepts.GroupKind;

public static class MagmaExtensions
{
	public static TElement Combine<TElement>(this IMagma<TElement> @this, TElement argument1, TElement argument2)
		where TElement : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(argument1, nameof(argument1));
		Preconditions.RequiresNotNull(argument2, nameof(argument2));

		return Postconditions.ReturnsNotNull(@this.Operation.Apply(argument1, argument2));
	}
}