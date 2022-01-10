namespace Common.Communication;

public interface IMessageHandler<TMessage>
	where TMessage : notnull
{
	public Task<HandleMessageResult> HandleMessageAsync(TMessage message);
}