using Common.Core.Diagnostics;
using Common.Core.MathConcepts.GroupKind;
using Common.Core.MathConcepts.ModuleKind;
using Common.Core.MathConcepts.RingKind;

namespace Common.Core.MathConcepts;

public static class ObjectMathExtensions
{
	public static T Combine<T>(this T @this, T other, IMagma<T> @group)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(@group, nameof(@group));

		return @group.Combine(@this, other);
	}

	public static T CombineInverse<T>(this T @this, T other, IGroup<T> @group)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(@group, nameof(@group));

		return @group.CombineInverse(@this, other);
	}

	public static T Inverse<T>(this T @this, IGroup<T> @group)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(@group, nameof(@group));

		return @group.Inverse(@this);
	}

	public static T Inverse<T>(this T @this, IField<T> field)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(field, nameof(field));

		return field.AdditiveInverse(@this);
	}

	public static T Reciprocal<T>(this T @this, IField<T> field)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(field, nameof(field));

		return field.MultiplicativeInverse(@this);
	}

	public static T Add<T>(this T @this, T other, IField<T> field)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(field, nameof(field));

		return field.Add(@this, other);
	}

	public static T Subtract<T>(this T @this, T other, IField<T> field)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(field, nameof(field));

		return field.Subtract(@this, other);
	}

	public static T Multiply<T>(this T @this, T other, IField<T> field)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(field, nameof(field));

		return field.Multiply(@this, other);
	}

	public static T Divide<T>(this T @this, T other, IField<T> field)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(field, nameof(field));

		return field.Divide(@this, other);
	}

	public static TVector Inverse<TVector, TScalar>(this TVector @this, ILinearSpace<TVector, TScalar> linearSpace)
		where TVector : notnull
		where TScalar : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(linearSpace, nameof(linearSpace));

		return linearSpace.VectorGroup.Inverse(@this);
	}

	public static TVector Reciprocal<TVector, TScalar>(
		this TVector @this,
		ILinearSpace<TVector, TScalar> linearSpace)
		where TVector : notnull
		where TScalar : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(linearSpace, nameof(linearSpace));

		var scalarField = linearSpace.ScalarField;
		return linearSpace.ScalarMultiply(
			scalarField.MultiplicativeInverse(scalarField.MultiplicativeIdentity),
			@this
		);
	}

	public static TVector Add<TVector, TScalar>(
		this TVector @this,
		TVector other,
		ILinearSpace<TVector, TScalar> linearSpace)
		where TVector : notnull
		where TScalar : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(linearSpace, nameof(linearSpace));

		return linearSpace.VectorGroup.Combine(@this, other);
	}

	public static TVector Subtract<TVector, TScalar>(
		this TVector @this,
		TVector other,
		ILinearSpace<TVector, TScalar> linearSpace)
		where TVector : notnull
		where TScalar : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(linearSpace, nameof(linearSpace));

		return linearSpace.VectorGroup.CombineInverse(@this, other);
	}

	public static TVector Multiply<TVector, TScalar>(
		this TVector @this,
		TScalar other,
		ILinearSpace<TVector, TScalar> linearSpace)
		where TVector : notnull
		where TScalar : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(linearSpace, nameof(linearSpace));

		return linearSpace.ScalarMultiply(other, @this);
	}

	public static TVector Divide<TVector, TScalar>(
		this TVector @this,
		TScalar other,
		ILinearSpace<TVector, TScalar> linearSpace)
		where TVector : notnull
		where TScalar : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(linearSpace, nameof(linearSpace));

		return linearSpace.ScalarMultiply(linearSpace.ScalarField.MultiplicativeInverse(other), @this);
	}
}