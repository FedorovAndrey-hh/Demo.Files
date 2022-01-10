namespace Common.Core.MathConcepts.Operations;

public interface IClosedBinaryOperation<TElement> : IBinaryOperation<TElement, TElement, TElement>
	where TElement : notnull
{
}