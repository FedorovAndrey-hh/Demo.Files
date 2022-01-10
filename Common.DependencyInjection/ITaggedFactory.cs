using Common.Core;

namespace Common.DependencyInjection;

// ReSharper disable once UnusedTypeParameter
public interface ITaggedFactory<out T, TTag> : IFactory<T>
	where T : notnull
	where TTag : notnull
{
}