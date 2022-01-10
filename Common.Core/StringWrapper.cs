namespace Common.Core;

public abstract record StringWrapper<TThis>
	where TThis : StringWrapper<TThis>
{
	protected StringWrapper(string value, StringComparison comparison)
	{
		Value = value;
		Comparison = comparison;
	}

	protected string Value { get; init; }

	protected StringComparison Comparison { get; }

	public virtual bool Equals(StringWrapper<TThis>? other)
		=> ReferenceEquals(other, this) || (other is TThis && string.Equals(Value, other.Value, Comparison));

	public override int GetHashCode() => Value.GetHashCode(Comparison);

	public override string ToString() => Value;
}