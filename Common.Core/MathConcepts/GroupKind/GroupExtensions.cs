using Common.Core.Diagnostics;

namespace Common.Core.MathConcepts.GroupKind;

public static class GroupExtensions
{
	public static TElement Inverse<TElement>(this IGroup<TElement> @this, TElement element)
		where TElement : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(element, nameof(element));

		return Postconditions.ReturnsNotNull(@this.InverseOperation.Apply(element));
	}

	public static TElement CombineInverse<TElement>(this IGroup<TElement> @this, TElement argument1, TElement argument2)
		where TElement : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(argument1, nameof(argument1));
		Preconditions.RequiresNotNull(argument2, nameof(argument2));

		return Postconditions.ReturnsNotNull(@this.Operation.Apply(argument1, @this.Inverse(argument2)));
	}
}