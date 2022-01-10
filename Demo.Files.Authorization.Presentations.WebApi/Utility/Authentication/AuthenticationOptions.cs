namespace Demo.Files.Authorization.Presentations.WebApi.Utility.Authentication;

public sealed class AuthenticationOptions
{
	public string? IssuerSigningKey { get; set; }
	public string? AccessTokenLifetime { get; set; }
}