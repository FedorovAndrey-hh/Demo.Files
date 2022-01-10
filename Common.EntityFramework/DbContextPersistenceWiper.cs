using Common.Core.Diagnostics;
using Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Common.EntityFramework;

public class DbContextPersistenceWiper : IPersistenceWiper
{
	public DbContextPersistenceWiper(DbContext dbContext)
	{
		Preconditions.RequiresNotNull(dbContext, nameof(dbContext));

		DbContext = dbContext;
	}

	protected DbContext DbContext { get; }

	public virtual Task WiperAsync() => DbContext.Database.EnsureDeletedAsync();
}