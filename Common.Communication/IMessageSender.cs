namespace Common.Communication;

public interface IMessageSender
{
	public Task SendMessageAsync(string port, object message);
}