namespace Common.Core;

public interface IConverter<in TSource, out TTarget>
{
	public TTarget Convert(TSource source);
}