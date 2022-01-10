using Common.Core.Diagnostics;
using Common.Core.Text;

namespace Common.Core.Emails;

public sealed class EmailFormatter : IExternalFormatter<Email>
{
	private static EmailFormatter? _cache;
	public static EmailFormatter Create() => _cache ?? (_cache = new EmailFormatter());

	private EmailFormatter()
	{
	}

	public string Format(Email data)
	{
		Preconditions.RequiresNotNull(data, nameof(data));

		return data.LocalPart + Email.DomainSeparator + data.Domain;
	}
}