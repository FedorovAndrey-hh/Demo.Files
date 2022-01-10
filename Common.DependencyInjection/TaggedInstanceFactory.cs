using Common.Core.Diagnostics;

namespace Common.DependencyInjection;

public sealed class TaggedInstanceFactory<T, TTag> : ITaggedFactory<T, TTag>
	where T : notnull
	where TTag : notnull
{
	public TaggedInstanceFactory(T instance)
	{
		Preconditions.RequiresNotNull(instance, nameof(instance));

		_instance = instance;
	}

	private readonly T _instance;

	public T Create() => _instance;
}