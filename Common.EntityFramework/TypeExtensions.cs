using System.Data.Common;
using Common.Core;
using Common.Core.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Common.EntityFramework;

public static class TypeExtensions
{
	public static bool IsEntityFrameworkException(this Type @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.IsSubclassOf<DbException>() || @this.IsSubclassOf<DbUpdateException>();
	}

	public static bool IsEntityFrameworkConcurrencyException(this Type @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.IsSubclassOf<DbUpdateConcurrencyException>();
	}
}