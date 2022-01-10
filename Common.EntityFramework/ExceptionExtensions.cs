using System.Data.Common;
using Common.Core.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Common.EntityFramework;

public static class ExceptionExtensions
{
	public static bool IsEntityFrameworkException(this Exception @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this is DbException || @this is DbUpdateException;
	}

	public static bool IsEntityFrameworkConcurrencyException(this Exception @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this is DbUpdateConcurrencyException;
	}
}