using Common.Core.Diagnostics;
using Common.Core.Work;
using Common.DependencyInjection;
using Demo.Files.Authorization.Domain.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity;

public static class ServiceCollectionExtensions
{
	public static void AddAspIdentityAuthorization<TDbContext>(this IServiceCollection @this)
		where TDbContext : AuthorizationDbContext
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this
			.AddScoped<IAsyncWorkScopeProvider<IAuthorizationContext>, AuthorizationWorkScopeProvider>()
			.AddScoped<User.IReadContext, UserReadContext>()
			.AddScoped<IAuthorizationPersistenceContext, AuthorizationPersistenceContext>()
			.AddScopedBinding<AuthorizationDbContext, TDbContext>()
			.AddSingleton<IUserValidator<UserData>>(UserValidator.Create())
			.AddSingleton<IPasswordValidator<UserData>>(UserPasswordValidator.Create())
			.AddIdentityCore<UserData>()
			.AddEntityFrameworkStores<AuthorizationDbContext>()
			.AddUserStore<InternalUserStore>();
	}
}