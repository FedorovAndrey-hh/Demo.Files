namespace Common.Core.Modifications;

public interface IVersion<TThis>
	: IEquatable<TThis>,
	  IComparable<TThis>
	where TThis : IVersion<TThis>
{
	public TThis Increment();
	public TThis Decrement();
}