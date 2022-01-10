namespace Common.Core.Events;

public delegate void StaticEventHandler();

public delegate void StaticEventHandler<in TEventArgs>(TEventArgs eventArgs)
	where TEventArgs : notnull;