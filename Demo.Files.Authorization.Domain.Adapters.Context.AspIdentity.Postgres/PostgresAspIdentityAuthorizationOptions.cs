﻿namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.Postgres;

public sealed class PostgresAspIdentityAuthorizationOptions
{
	public string? Server { get; set; }
	public uint? Port { get; set; }
	public string? Database { get; set; }
	public string? Username { get; set; }
	public string? Password { get; set; }
}