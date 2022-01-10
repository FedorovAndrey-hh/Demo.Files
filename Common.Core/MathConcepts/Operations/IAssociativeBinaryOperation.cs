namespace Common.Core.MathConcepts.Operations;

public interface IAssociativeBinaryOperation<in TArgument1, in TArgument2, out TResult>
	: IBinaryOperation<TArgument1, TArgument2, TResult>
	where TArgument1 : notnull
	where TArgument2 : notnull
	where TResult : notnull
{
	public TResult Apply(TArgument2 left, TArgument1 right) => Apply(right, left);
}