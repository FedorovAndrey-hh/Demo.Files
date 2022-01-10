namespace Common.Core.Events;

public delegate void EventHandler<in TSender>(TSender sender)
	where TSender : notnull;

public delegate void EventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs eventArgs)
	where TSender : notnull
	where TEventArgs : notnull;