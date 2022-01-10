namespace Common.Core;

public interface IBiConverter<T1, T2>
{
	public IConverter<T1, T2> Forward { get; }

	public IConverter<T2, T1> Backward { get; }
}