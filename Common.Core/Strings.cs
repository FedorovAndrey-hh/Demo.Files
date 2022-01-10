using Common.Core.Diagnostics;

namespace Common.Core;

public static class Strings
{
	public static string JoinSkipNulls(string? separator, params string?[] values)
	{
		Preconditions.RequiresNotNull(values, nameof(values));

		return string.Join(separator, values.Where(e => e is not null));
	}
}