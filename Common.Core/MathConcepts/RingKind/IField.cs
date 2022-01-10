using Common.Core.MathConcepts.Operations;

namespace Common.Core.MathConcepts.RingKind;

public interface IField<TElement>
	where TElement : notnull
{
	public interface IAdditionOperation
		: IAssociativeBinaryOperation<TElement, TElement, TElement>,
		  ICommutativeBinaryOperation<TElement, TElement, TElement>,
		  IClosedBinaryOperation<TElement>
	{
	}

	public IAdditionOperation AdditionOperation { get; }

	public interface IAdditionInverseOperation : IUnaryOperation<TElement>
	{
	}

	public IAdditionInverseOperation AdditiveInverseOperation { get; }

	public interface IMultiplicationOperation
		: IAssociativeBinaryOperation<TElement, TElement, TElement>,
		  ICommutativeBinaryOperation<TElement, TElement, TElement>,
		  IClosedBinaryOperation<TElement>
	{
	}

	public TElement AdditiveIdentity { get; }

	public IMultiplicationOperation MultiplicationOperation { get; }

	public interface IMultiplicativeInverseOperation : IUnaryOperation<TElement>
	{
	}

	public IMultiplicativeInverseOperation MultiplicativeInverseOperation { get; }

	public TElement MultiplicativeIdentity { get; }
}