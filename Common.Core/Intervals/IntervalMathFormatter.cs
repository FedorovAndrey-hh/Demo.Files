using System.Text;
using Common.Core.Diagnostics;
using Common.Core.Text;

namespace Common.Core.Intervals;

public sealed class IntervalMathFormatter<T> : IExternalFormatter<IInterval<T>>
	where T : notnull
{
	public IntervalMathFormatter(IExternalFormatter<T>? limitFormatter)
	{
		_limitFormatter = limitFormatter;
	}

	private readonly IExternalFormatter<T>? _limitFormatter;

	public string Format(IInterval<T> data)
	{
		Preconditions.RequiresNotNull(data, nameof(data));

		var result = new StringBuilder();

		var start = data.Start;
		if (start is null)
		{
			result.Append("(;");
		}
		else
		{
			result.Append(start.IsInclusive ? "[" : "(");
			result.Append(_limitFormatter is null ? start.Value.ToString() : _limitFormatter.Format(start.Value));
			result.Append(";");
		}

		var end = data.End;
		if (end is null)
		{
			result.Append(")");
		}
		else
		{
			result.Append(" ");
			result.Append(_limitFormatter is null ? end.Value.ToString() : _limitFormatter.Format(end.Value));
			result.Append(end.IsInclusive ? "]" : ")");
		}

		return result.ToString();
	}
}