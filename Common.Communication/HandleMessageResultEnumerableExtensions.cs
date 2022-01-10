namespace Common.Communication;

public static class HandleMessageResultEnumerableExtensions
{
	public static HandleMessageResult Combine(this HandleMessageResult @this, HandleMessageResult other)
	{
		if (@this is HandleMessageResult.Invalid || other is HandleMessageResult.Invalid)
		{
			return HandleMessageResult.Invalid;
		}

		if (@this is HandleMessageResult.CurrentlyUnprocessable || other is HandleMessageResult.CurrentlyUnprocessable)
		{
			return HandleMessageResult.CurrentlyUnprocessable;
		}

		return HandleMessageResult.Success;
	}
}