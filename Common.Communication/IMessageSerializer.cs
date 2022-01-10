namespace Common.Communication;

public interface IMessageSerializer
{
	public byte[] SerializeMessage(object message);

	public T DeserializeMessage<T>( byte[] message)
		where T : notnull;
}