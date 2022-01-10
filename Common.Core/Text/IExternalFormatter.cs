namespace Common.Core.Text;

public interface IExternalFormatter<in TFormatData>
	where TFormatData : notnull
{
	public string Format(TFormatData data);
}