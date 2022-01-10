using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Common.Core;
using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;
using Demo.Files.Common.Contracts.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Demo.Files.Authorization.Presentations.WebApi.Utility.Authentication;

public sealed class JwtAccessTokenFactory : IAccessTokenFactory
{
	public JwtAccessTokenFactory(
		string? issuer,
		string? audience,
		TimeSpan lifetime,
		SecurityKey signingCredentials)
	{
		Preconditions.RequiresNotNull(signingCredentials, nameof(signingCredentials));

		_issuer = issuer;
		_audience = audience;
		_lifetime = lifetime;
		_signingCredentials = signingCredentials;
	}

	private readonly string? _issuer;
	private readonly string? _audience;
	private readonly TimeSpan _lifetime;
	private readonly SecurityKey _signingCredentials;

	public Task<string> CreateTokenAsync(User user)
	{
		Preconditions.RequiresNotNull(user, nameof(user));

		var claims = new LinkedList<Claim>();
		claims.AddLast(new Claim(JwtRegisteredClaimNames.Sub, user.Id.RawLong().ToString()));
		foreach (var resource in user.Resources)
		{
			if (user.CanAccessResource(resource))
			{
				var claimType = resource.Type switch
				{
					ResourceType.Storage => FilesClaimNames.Storage,
					_ => throw Errors.UnsupportedEnumValue(resource.Type)
				};

				var claimValue = resource switch
				{
					Resource.Storage storage => storage.Id.RawLong().ToString(),
					_ => throw Errors.UnsupportedType(resource.GetType())
				};

				claims.AddLast(new Claim(claimType, claimValue));
			}
		}

		var now = DateTime.UtcNow;
		var token = new JwtSecurityToken(
			issuer: _issuer,
			audience: _audience,
			notBefore: now,
			claims: claims,
			expires: now.Add(_lifetime),
			signingCredentials: new SigningCredentials(
				_signingCredentials,
				SecurityAlgorithms.HmacSha256
			)
		);
		return new JwtSecurityTokenHandler().WriteToken(token).AsTaskResult();
	}
}