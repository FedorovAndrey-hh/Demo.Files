namespace Common.Core;

public interface IFactory<out T>
	where T : notnull
{
	public T Create();
}