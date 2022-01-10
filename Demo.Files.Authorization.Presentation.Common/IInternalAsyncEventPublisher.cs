using Common.Core.Events;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Presentation.Common;

public interface IInternalAsyncEventPublisher : IAsyncEventPublisher<UserEvent.Modified.ResourceRequested>
{
}