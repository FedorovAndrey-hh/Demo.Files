using Common.Core.Diagnostics;

namespace Common.EntityFramework.Postgres;

public sealed class PostgresConnectionInfo
{
	public PostgresConnectionInfo(
		string server,
		uint port,
		string database,
		string username,
		string password)
	{
		Preconditions.RequiresNotNull(server, nameof(server));
		Preconditions.RequiresNotNull(database, nameof(database));
		Preconditions.RequiresNotEmpty(database, nameof(database));
		Preconditions.RequiresNotNull(username, nameof(username));
		Preconditions.RequiresNotEmpty(username, nameof(username));
		Preconditions.RequiresNotNull(password, nameof(password));
		Preconditions.RequiresNotEmpty(password, nameof(password));

		Server = server;
		Port = port;
		Database = database;
		Username = username;
		Password = password;
	}

	public string Server { get; }
	public uint Port { get; }
	public string Database { get; }
	public string Username { get; }
	public string Password { get; }

	public string GetConnectionString()
		=> $"Server={Server};Port={Port};Database={Database};User Id={Username};Password={Password};";
}