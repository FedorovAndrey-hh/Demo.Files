namespace Common.Core.Modifications;

public interface IContinuous<out TThis, in TEvent>
	where TThis : IContinuous<TThis, TEvent>
	where TEvent : notnull
{
	public TThis After(TEvent @event);
}