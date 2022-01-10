using System.Collections.Immutable;
using Common.Core.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Common.EntityFramework;

public static class QueryableExtensions
{
	public static async Task<IImmutableList<TSource>> ToImmutableListAsync<TSource>(
		this IQueryable<TSource> source,
		CancellationToken cancellationToken = default)
	{
		Preconditions.RequiresNotNull(source, nameof(source));

		var result = ImmutableList.CreateBuilder<TSource>();
		await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
		{
			result.Add(element);
		}

		return result.ToImmutable();
	}
}