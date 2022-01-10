using Common.Core.MathConcepts.Operations;

namespace Common.Core.MathConcepts.GroupKind;

public interface ISemigroup<TElement> : IMagma<TElement>
	where TElement : notnull
{
	public interface IOperation
		: IMagma<TElement>.IOperation,
		  IAssociativeBinaryOperation<TElement, TElement, TElement>
	{
	}

	public new IOperation Operation { get; }

	IMagma<TElement>.IOperation IMagma<TElement>.Operation => Operation;
}