using Common.Core.MathConcepts.Operations;

namespace Common.Core.MathConcepts.GroupKind;

public interface IGroup<TElement> : IMonoid<TElement>
	where TElement : notnull
{
	public interface IInverseOperation : IUnaryOperation<TElement>
	{
	}

	public IInverseOperation InverseOperation { get; }
}