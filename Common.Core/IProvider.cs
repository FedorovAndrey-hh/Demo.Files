namespace Common.Core;

public interface IProvider<out T>
	where T : notnull
{
	public T Get();
}