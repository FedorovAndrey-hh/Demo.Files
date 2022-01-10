using Common.Communication;
using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Execution.Retry;
using Demo.Files.Common.Contracts.Communication.Authorization;
using Demo.Files.FilesManagement.Applications.Abstractions;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Demo.Files.FilesManagement.Presentation.Common;
using Microsoft.Extensions.Logging;

namespace Demo.Files.FilesManagement.Presentations.Internal;

public sealed class RequestPremiumStorageMessageHandler : IMessageHandler<RequestPremiumStorageMessage>
{
	public RequestPremiumStorageMessageHandler(
		ICreatePremiumStorage useCase,
		IMessageSender messageSender,
		IDelayedRetryPolicy retryPolicy,
		ILogger<RequestPremiumStorageMessageHandler> logger)
	{
		Preconditions.RequiresNotNull(useCase, nameof(useCase));
		Preconditions.RequiresNotNull(messageSender, nameof(messageSender));
		Preconditions.RequiresNotNull(retryPolicy, nameof(retryPolicy));
		Preconditions.RequiresNotNull(logger, nameof(logger));

		_useCase = useCase;
		_messageSender = messageSender;
		_logger = logger;
		_retryPolicy = retryPolicy;
	}

	private readonly ICreatePremiumStorage _useCase;
	private readonly IMessageSender _messageSender;
	private readonly IDelayedRetryPolicy _retryPolicy;
	private readonly ILogger<RequestPremiumStorageMessageHandler> _logger;

	public async Task<HandleMessageResult> HandleMessageAsync(RequestPremiumStorageMessage message)
	{
		Preconditions.RequiresNotNull(message, nameof(message));

		_logger.LogInformation("Message received ({MessageId})", message.Id);

		try
		{
			var retryScope = _retryPolicy.CreateScope();

			Storage storage;

			while (true)
			{
				try
				{
					storage = await _useCase.ExecuteAsync().ConfigureAwait(false);

					break;
				}
				catch (Exception e) when (retryScope.ShouldRetry(e, out var retryDelay))
				{
					_logger.LogInformation(e, "Retry to handle message");

					await retryDelay.ToDelayTask().ConfigureAwait(false);
				}
			}

			// TODO: Guarantee delivery.
			await _messageSender.SendMessageAsync(
					PremiumStorageCreatedMessage.Port,
					new PremiumStorageCreatedMessage(
						message.Id,
						storage.Id.ToCommunicationContext()
					)
				)
				.ConfigureAwait(false);

			return HandleMessageResult.Success;
		}
		catch (Exception e)
		{
			_logger.LogCritical(e, "Failed to handle message");
			throw;
		}
	}
}