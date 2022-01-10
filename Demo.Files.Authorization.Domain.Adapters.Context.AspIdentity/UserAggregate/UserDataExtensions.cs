using System.Collections.Immutable;
using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Emails;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public static class UserDataExtensions
{
	public static UserId GetId(this UserData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return UserId.Of(@this.Id);
	}

	public static void SetUsername(this UserData @this, Username username)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.UserName = username.ToString(Username.Formatter);
	}

	public static Username GetUsername(this UserData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return Username.Parser.Parse(@this.UserName, out var result, out var error)
			? result
			: throw new UserException(error);
	}

	public static void SetEmail(this UserData @this, UserEmail email)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Email = email.Value.ToString(Email.Formatter);
	}

	public static UserEmail GetEmail(this UserData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return UserEmail.Parser.Parse(@this.Email, out var result, out var error)
			? result
			: throw new UserException(error);
	}

	public static void SetVersion(this UserData @this, UserVersion version)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Version = version.Value;
	}

	public static UserVersion GetVersion(this UserData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return UserVersion.Of(@this.Version);
	}

	public static IImmutableSet<Resource> GetResources(this UserData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		var resources = @this.Resources;
		if (resources is null)
		{
			return ImmutableHashSet.Create<Resource>();
		}
		else
		{
			return resources.Split(",")
				.Select(
					e =>
					{
						var parts = e.Split(_resourcePartsSeparator, 4);
						Contracts.Assert(parts.Length == 3, "Invalid resource format.");

						Resource result = parts[0] switch
						{
							"S" => new Resource.Storage(
								StorageId.Of(
									long.TryParse(parts[1], out var rawStorageId)
										? rawStorageId
										: throw new ContractException("Invalid resource format.")
								),
								ResourceRequestId.Of(
									long.TryParse(parts[2], out var rawResourceRequestId)
										? rawResourceRequestId
										: throw new ContractException("Invalid resource format.")
								)
							),
							_ => throw new ContractException("Invalid resource format.")
						};
						return result;
					}
				)
				.ToImmutableHashSet();
		}
	}

	public static void SetResources(this UserData @this, IImmutableList<Resource> resources)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(resources, nameof(resources));

		@this.Resources = string.Join(
			_resourceSeparator,
			resources.Select(e => _ResourceToString(e))
		);
	}

	public static void AddResource(this UserData @this, Resource resource)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(resource, nameof(resource));

		@this.Resources =
			@this.Resources.IsNullOrEmpty()
				? _ResourceToString(resource)
				: @this.Resources + _resourceSeparator + _ResourceToString(resource);
	}

	public static void RemoveResource(this UserData @this, Resource resource)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(resource, nameof(resource));

		var resourceAsString = _ResourceToString(resource);
		@this.Resources = @this.Resources
			?.Replace(resourceAsString + _resourceSeparator, null, StringComparison.Ordinal)
			.Replace(resourceAsString, null, StringComparison.Ordinal);
	}

	private const string _resourceSeparator = ",";
	private const string _resourcePartsSeparator = ":";

	private static string _ResourceToString(Resource resource)
	{
		Preconditions.RequiresNotNull(resource, nameof(resource));

		return resource.Switch(
			storage =>
			{
				var rawStorageId = storage.Id.RawLong();
				var rawResourceRequestId = storage.RequestId.RawLong();
				return $"S{_resourcePartsSeparator}{rawStorageId}{_resourcePartsSeparator}{rawResourceRequestId}";
			}
		);
	}
}