using Common.Communication;
using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Execution.Retry;
using Demo.Files.Authorization.Applications.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Authorization.Presentation.Common;
using Demo.Files.Common.Contracts.Communication.Authorization;
using Microsoft.Extensions.Logging;

namespace Demo.Files.Authorization.Presentations.Internal;

public sealed class PremiumStorageCreatedMessageHandler : IMessageHandler<PremiumStorageCreatedMessage>
{
	public PremiumStorageCreatedMessageHandler(
		IAsUserAcquireStorageResource useCase,
		IDelayedRetryPolicy retryPolicy,
		ILogger<PremiumStorageCreatedMessageHandler> logger)
	{
		Preconditions.RequiresNotNull(useCase, nameof(useCase));
		Preconditions.RequiresNotNull(retryPolicy, nameof(retryPolicy));
		Preconditions.RequiresNotNull(logger, nameof(logger));

		_useCase = useCase;
		_retryPolicy = retryPolicy;
		_logger = logger;
	}

	private readonly IAsUserAcquireStorageResource _useCase;
	private readonly IDelayedRetryPolicy _retryPolicy;
	private readonly ILogger<PremiumStorageCreatedMessageHandler> _logger;

	public async Task<HandleMessageResult> HandleMessageAsync(PremiumStorageCreatedMessage message)
	{
		Preconditions.RequiresNotNull(message, nameof(message));

		_logger.LogInformation("Message received ({MessageId})", message.Id);

		var (userId, resourceRequestId)
			= CommunicationConverters.RequestPremiumStorageId.ConvertBackward(
				message.Id ?? throw new ContractException($"Invalid message id ({message.Id}).")
			);
		var storageId = (message.StorageId ?? throw new ContractException($"Invalid storage id ({message.StorageId})."))
			.ToAuthorizationContext();

		try
		{
			var retryScope = _retryPolicy.CreateScope();

			while (true)
			{
				try
				{
					await _useCase
						.ExecuteAsync(
							userId,
							null,
							new Resource.Storage(storageId, resourceRequestId)
						)
						.ConfigureAwait(false);

					break;
				}
				catch (UserException e) when (e.Error == UserError.ResourceAlreadyAcquired)
				{
					_logger.LogInformation(e, "Resource already acquired");
				}
				catch (Exception e) when (retryScope.ShouldRetry(e, out var retryDelay))
				{
					_logger.LogInformation(e, "Retry to handle message");

					await retryDelay.ToDelayTask().ConfigureAwait(false);
				}
			}

			return HandleMessageResult.Success;
		}
		catch (Exception e)
		{
			_logger.LogCritical(e, "Failed to handle message ({MessageId})", message.Id);
			throw;
		}
	}
}