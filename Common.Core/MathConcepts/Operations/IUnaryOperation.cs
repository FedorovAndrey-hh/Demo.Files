namespace Common.Core.MathConcepts.Operations;

public interface IUnaryOperation<TElement>
	where TElement : notnull
{
	TElement Apply(TElement element);
}