using Common.Core.Diagnostics;
using Common.Core.Environments;
using Microsoft.EntityFrameworkCore;

namespace Common.Persistence.EntityFramework;

public sealed class EntityFrameworkEnvironmentStateDetector : PeriodicEnvironmentStateDetector
{
	public EntityFrameworkEnvironmentStateDetector(TimeSpan checkPeriod, DbContext dbContext)
		: base(checkPeriod)
	{
		Preconditions.RequiresNotNull(dbContext, nameof(dbContext));

		_dbContext = dbContext;
	}

	private readonly DbContext _dbContext;

	protected override Task<bool> CheckAsync() => _dbContext.Database.CanConnectAsync();
}