using Common.Core.Diagnostics;
using Demo.Files.Common.Contracts.Authorization;
using Demo.Files.Common.Web;

namespace Demo.Files.Authorization.Presentations.WebApi.Utility.Authentication;

public static class AuthenticationExtensions
{
	public static void AddFilesProviderAuthentication(this IServiceCollection @this, IConfiguration configuration)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(@this, nameof(configuration));

		const string issuer = FilesAccessTokenMetadata.Issuer;
		const string audience = FilesAccessTokenMetadata.Audience;
		var authenticationOptions =
			configuration
				.GetSection(nameof(AuthenticationOptions))
				.Get<AuthenticationOptions>()
			?? throw Errors.EnvironmentVariableNotFound(nameof(AuthenticationOptions));

		var rawIssuerSigningKey =
			authenticationOptions.IssuerSigningKey
			?? throw Errors.EnvironmentVariableNotFound(nameof(AuthenticationOptions.IssuerSigningKey));

		var accessTokenLifetime =
			TimeSpan.TryParse(authenticationOptions.AccessTokenLifetime, out var result)
				? result
				: throw Errors.EnvironmentVariableNotFound(nameof(AuthenticationOptions.AccessTokenLifetime));

		@this.AddFilesConsumerAuthentication(rawIssuerSigningKey, out var issuerSigningKey);

		@this.AddSingleton<IAccessTokenFactory>(
			new JwtAccessTokenFactory(
				issuer,
				audience,
				accessTokenLifetime,
				issuerSigningKey
			)
		);
	}

	public static void UseFilesProviderAuthentication(this IApplicationBuilder @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.UseFilesConsumerAuthentication();
	}
}