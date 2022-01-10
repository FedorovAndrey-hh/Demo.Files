using System.Collections.Immutable;
using Common.Core.Diagnostics;

namespace Common.Core.Text;

public sealed class CompositeFormatProvider : IFormatProvider
{
	public static CompositeFormatProvider Empty { get; } = new(ImmutableList<IFormatProvider>.Empty);

	public CompositeFormatProvider(params IFormatProvider[] providers)
		: this(providers.AsEnumerable())
	{
	}

	public CompositeFormatProvider(IEnumerable<IFormatProvider> providers)
		: this(providers, ImmutableList<IFormatProvider>.Empty)
	{
	}

	private CompositeFormatProvider(
		IEnumerable<IFormatProvider> providers,
		ImmutableList<IFormatProvider> nonCompositeProviders)
	{
		Preconditions.RequiresNotNull(providers, nameof(providers));
		Preconditions.RequiresNotNull(nonCompositeProviders, nameof(nonCompositeProviders));

		var aggregatedProviders = nonCompositeProviders;
		foreach (var provider in providers)
		{
			if (Preconditions.RequiresNotNull(provider, nameof(provider)) is CompositeFormatProvider composite)
			{
				aggregatedProviders = aggregatedProviders.AddRange(composite._providers);
			}
			else
			{
				aggregatedProviders = aggregatedProviders.Add(provider);
			}
		}

		_providers = aggregatedProviders;
	}

	private readonly ImmutableList<IFormatProvider> _providers;

	public object? GetFormat(Type? formatType)
	{
		foreach (var provider in _providers)
		{
			var format = provider.GetFormat(formatType);
			if (format is not null)
			{
				return format;
			}
		}

		return null;
	}

	public CompositeFormatProvider AddProvider(params IFormatProvider[] providers)
	{
		Preconditions.RequiresNotNull(providers, nameof(providers));

		return new CompositeFormatProvider(providers, _providers);
	}

	public CompositeFormatProvider AddProviders(IEnumerable<IFormatProvider> providers)
	{
		Preconditions.RequiresNotNull(providers, nameof(providers));

		return new CompositeFormatProvider(providers, _providers);
	}
}