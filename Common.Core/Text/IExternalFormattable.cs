namespace Common.Core.Text;

public interface IExternalFormattable<out TFormatData>
	where TFormatData : notnull
{
	public string ToString(IExternalFormatter<TFormatData> formatter);
}