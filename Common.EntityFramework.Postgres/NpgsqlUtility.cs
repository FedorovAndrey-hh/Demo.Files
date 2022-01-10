using Common.Core.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.EntityFramework.Postgres;

public static class NpgsqlUtility
{
	public static PropertyBuilder<uint> IsXminConcurrencyToken(this PropertyBuilder<uint> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.HasColumnName("xmin").HasColumnType("xid").ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();
	}
}