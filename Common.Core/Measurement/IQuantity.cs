namespace Common.Core.Measurement;

public interface IQuantity<out TValue, out TUnit>
	where TValue : notnull
{
	public TValue Value { get; }

	public TUnit Unit { get; }
}