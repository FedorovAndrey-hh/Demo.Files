using Common.Core.MathConcepts.GroupKind;
using Common.Core.MathConcepts.Operations;
using Common.Core.MathConcepts.RingKind;

namespace Common.Core.MathConcepts.ModuleKind;

public interface ILinearSpace<TVector, TScalar>
	where TVector : notnull
	where TScalar : notnull
{
	public IGroup<TVector> VectorGroup { get; }

	public IField<TScalar> ScalarField { get; }

	public interface IScalarMultiplicationOperation : ICommutativeBinaryOperation<TScalar, TVector, TVector>
	{
	}

	public IScalarMultiplicationOperation ScalarMultiplicationOperation { get; }
}