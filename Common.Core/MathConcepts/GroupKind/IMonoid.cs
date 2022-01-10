namespace Common.Core.MathConcepts.GroupKind;

public interface IMonoid<TElement> : ISemigroup<TElement>
	where TElement : notnull
{
	public TElement Identity { get; }
}