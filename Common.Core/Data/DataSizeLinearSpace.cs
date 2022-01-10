using Common.Core.Diagnostics;
using Common.Core.MathConcepts.GroupKind;
using Common.Core.MathConcepts.ModuleKind;
using Common.Core.MathConcepts.RingKind;

namespace Common.Core.Data;

internal sealed class DataSizeLinearSpace<TValue>
	: ILinearSpace<DataSize<TValue>, TValue>,
	  ILinearSpace<DataSize<TValue>, TValue>.IScalarMultiplicationOperation
	where TValue : notnull
{
	public DataSizeLinearSpace(IGroup<DataSize<TValue>> vectorGroup, IField<TValue> scalarField)
	{
		Preconditions.RequiresNotNull(vectorGroup, nameof(vectorGroup));
		Preconditions.RequiresNotNull(scalarField, nameof(scalarField));

		VectorGroup = vectorGroup;
		ScalarField = scalarField;
	}

	public IGroup<DataSize<TValue>> VectorGroup { get; }
	public IField<TValue> ScalarField { get; }

	public DataSize<TValue> Apply(TValue left, DataSize<TValue> right)
	{
		Preconditions.RequiresNotNull(left, nameof(left));
		Preconditions.RequiresNotNull(right, nameof(right));

		return new DataSize<TValue>(ScalarField.Multiply(left, right.Value), right.Unit);
	}

	public ILinearSpace<DataSize<TValue>, TValue>.IScalarMultiplicationOperation ScalarMultiplicationOperation
		=> this;
}