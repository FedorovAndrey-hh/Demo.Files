namespace Common.Core.MathConcepts.Operations;

public interface ICommutativeBinaryOperation<in TLeft, in TRight, out TResult>
	: IBinaryOperation<TLeft, TRight, TResult>
	where TLeft : notnull
	where TRight : notnull
	where TResult : notnull
{
}