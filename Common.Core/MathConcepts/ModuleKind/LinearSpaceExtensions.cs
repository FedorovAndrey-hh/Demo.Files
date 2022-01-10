using Common.Core.Diagnostics;

namespace Common.Core.MathConcepts.ModuleKind;

public static class LinearSpaceExtensions
{
	public static TVector ScalarMultiply<TVector, TScalar>(
		this ILinearSpace<TVector, TScalar> @this,
		TScalar scalar,
		TVector vector)
		where TVector : notnull
		where TScalar : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(scalar, nameof(scalar));
		Preconditions.RequiresNotNull(vector, nameof(vector));

		return Postconditions.ReturnsNotNull(@this.ScalarMultiplicationOperation.Apply(scalar, vector));
	}
}