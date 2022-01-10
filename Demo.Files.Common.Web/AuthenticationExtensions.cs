using System.Text;
using Common.Core.Diagnostics;
using Demo.Files.Common.Contracts.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Demo.Files.Common.Web;

public static class AuthenticationExtensions
{
	public static void AddFilesConsumerAuthentication(
		this IServiceCollection @this,
		IConfiguration configuration)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(configuration, nameof(configuration));

		var authenticationOptions =
			configuration
				.GetSection(nameof(AuthenticationConsumerOptions))
				.Get<AuthenticationConsumerOptions>()
			?? throw Errors.EnvironmentVariableNotFound(nameof(AuthenticationConsumerOptions));

		var rawIssuerSigningKey =
			authenticationOptions.IssuerSigningKey
			?? throw Errors.EnvironmentVariableNotFound(nameof(AuthenticationConsumerOptions.IssuerSigningKey));

		@this.AddFilesConsumerAuthentication(rawIssuerSigningKey, out _);
	}

	public static void AddFilesConsumerAuthentication(
		this IServiceCollection @this,
		string rawIssuerSigningKey,
		out SymmetricSecurityKey issuerSigningKey)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(rawIssuerSigningKey, nameof(rawIssuerSigningKey));

		const string issuer = FilesAccessTokenMetadata.Issuer;
		const string audience = FilesAccessTokenMetadata.Audience;

		var issuerSigningKeyLocal = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(rawIssuerSigningKey));
		issuerSigningKey = issuerSigningKeyLocal;

		@this
			.AddAuthentication(
				options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				}
			)
			.AddJwtBearer(
				options =>
				{
					options.RequireHttpsMetadata = false;
					options.SaveToken = true;

					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidIssuer = issuer,
						ValidAudience = audience,
						IssuerSigningKey = issuerSigningKeyLocal
					};
				}
			);
	}

	public static void UseFilesConsumerAuthentication(this IApplicationBuilder @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.UseAuthentication();
	}
}