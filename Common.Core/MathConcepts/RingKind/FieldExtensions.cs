using Common.Core.Diagnostics;

namespace Common.Core.MathConcepts.RingKind;

public static class FieldExtensions
{
	public static TElement Add<TElement>(this IField<TElement> @this, TElement left, TElement right)
		where TElement : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(left, nameof(left));
		Preconditions.RequiresNotNull(right, nameof(right));

		return Postconditions.ReturnsNotNull(@this.AdditionOperation.Apply(left, right));
	}

	public static TElement Subtract<TElement>(this IField<TElement> @this, TElement left, TElement right)
		where TElement : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(left, nameof(left));
		Preconditions.RequiresNotNull(right, nameof(right));

		return Postconditions.ReturnsNotNull(@this.AdditionOperation.Apply(left, @this.AdditiveInverse(right)));
	}

	public static TElement AdditiveInverse<TElement>(this IField<TElement> @this, TElement element)
		where TElement : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(element, nameof(element));

		return Postconditions.ReturnsNotNull(@this.AdditiveInverseOperation.Apply(element));
	}

	public static TElement Multiply<TElement>(this IField<TElement> @this, TElement left, TElement right)
		where TElement : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(left, nameof(left));
		Preconditions.RequiresNotNull(right, nameof(right));

		return Postconditions.ReturnsNotNull(@this.MultiplicationOperation.Apply(left, right));
	}

	public static TElement Divide<TElement>(this IField<TElement> @this, TElement left, TElement right)
		where TElement : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(left, nameof(left));
		Preconditions.RequiresNotNull(right, nameof(right));

		return Postconditions.ReturnsNotNull(
			@this.MultiplicationOperation.Apply(left, @this.MultiplicativeInverse(right))
		);
	}

	public static TElement MultiplicativeInverse<TElement>(this IField<TElement> @this, TElement element)
		where TElement : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(element, nameof(element));

		return Postconditions.ReturnsNotNull(@this.MultiplicativeInverseOperation.Apply(element));
	}
}