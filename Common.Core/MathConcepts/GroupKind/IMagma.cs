using Common.Core.MathConcepts.Operations;

namespace Common.Core.MathConcepts.GroupKind;

public interface IMagma<TElement>
	where TElement : notnull
{
	public interface IOperation : IClosedBinaryOperation<TElement>
	{
	}

	public IOperation Operation { get; }
}