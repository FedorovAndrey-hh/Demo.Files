namespace Common.Core.Measurement;

public interface IMeasurer<TValue, in TUnit>
	where TValue : notnull
	where TUnit : notnull
{
	public TValue Measure(TValue sourceValue, TUnit sourceUnit, TUnit targetUnit);
}