using Common.Core.Diagnostics;

namespace Common.Core;

public static class Programs
{
	private static void _OnCancelOrProcessExitDoOnce(Action action)
	{
		Preconditions.RequiresNotNull(action, nameof(action));

		ConsoleCancelEventHandler? cancelHandler = null;
		cancelHandler = (_, eventArgs) =>
		{
			eventArgs.Cancel = true;

			action();

			Console.CancelKeyPress -= cancelHandler;
		};
		Console.CancelKeyPress += cancelHandler;

		EventHandler? exitHandler = null;
		exitHandler = (_, _) =>
		{
			action();

			AppDomain.CurrentDomain.ProcessExit -= exitHandler;
		};
		AppDomain.CurrentDomain.ProcessExit += exitHandler;
	}

	public static Task WaitForCancelOrProcessExitAsync()
	{
		var taskSource = new TaskCompletionSource();

		_OnCancelOrProcessExitDoOnce(() => taskSource.TrySetResult());

		return taskSource.Task;
	}

	public static CancellationToken CancelOrProcessExitCancellationToken()
	{
		var tokenSource = new CancellationTokenSource();

		_OnCancelOrProcessExitDoOnce(tokenSource.Cancel);

		return tokenSource.Token;
	}
}