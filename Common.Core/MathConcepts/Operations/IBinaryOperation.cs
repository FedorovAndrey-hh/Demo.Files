namespace Common.Core.MathConcepts.Operations;

public interface IBinaryOperation<in TLeft, in TRight, out TResult>
	where TLeft : notnull
	where TRight : notnull
	where TResult : notnull
{
	public TResult Apply(TLeft left, TRight right);
}