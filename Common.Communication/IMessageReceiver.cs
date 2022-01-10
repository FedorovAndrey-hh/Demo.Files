namespace Common.Communication;

public interface IMessageReceiver
{
	public void OnReceiveMessage<TMessage>(string port, IMessageHandler<TMessage> handler)
		where TMessage : notnull;
}