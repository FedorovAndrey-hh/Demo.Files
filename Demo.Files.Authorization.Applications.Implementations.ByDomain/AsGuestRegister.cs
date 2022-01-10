using Common.Core.Diagnostics;
using Common.Core.Work;
using Demo.Files.Authorization.Applications.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Applications.Implementations.ByDomain;

public sealed class AsGuestRegister : IAsGuestRegister
{
	public AsGuestRegister(IAsyncWorkScopeProvider<IAuthorizationContext> workScopeProvider)
	{
		Preconditions.RequiresNotNull(workScopeProvider, nameof(workScopeProvider));

		_workScopeProvider = workScopeProvider;
	}

	private readonly IAsyncWorkScopeProvider<IAuthorizationContext> _workScopeProvider;

	public Task<User> ExecuteAsync(string email, string displayName, string password)
		=> _workScopeProvider.WithinScopeDoAsync(
			context => _ExecuteAsync(
				context.ForUser(),
				email,
				displayName,
				password
			)
		);

	private async Task<User> _ExecuteAsync(
		User.IWriteContext context,
		string email,
		string displayName,
		string password)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		return User.After(
			await User
				.RegisterAsync(
					context,
					UserEmail.Create(email),
					UserDisplayName.Create(displayName),
					Password.Create(password)
				)
				.ConfigureAwait(false)
		);
	}
}