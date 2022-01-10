using Common.Core.Diagnostics;
using Common.Core.Environments;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using TaskExtensions = Common.Core.TaskExtensions;

namespace Common.Communication.RabbitMq;

public sealed class RabbitMqEnvironmentStateDetector : PeriodicEnvironmentStateDetector
{
	private readonly IConnectionFactory _connectionFactory;

	public RabbitMqEnvironmentStateDetector(TimeSpan checkPeriod, IConnectionFactory connectionFactory)
		: base(checkPeriod)
	{
		Preconditions.RequiresNotNull(connectionFactory, nameof(connectionFactory));

		_connectionFactory = connectionFactory;
	}

	protected override Task<bool> CheckAsync()
	{
		try
		{
			_connectionFactory.CreateConnection().Dispose();
			return TaskExtensions.TrueResult;
		}
		catch (BrokerUnreachableException)
		{
			return TaskExtensions.FalseResult;
		}
	}
}