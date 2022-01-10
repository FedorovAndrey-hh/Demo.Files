using System.Reflection;
using Common.Core;
using Common.Core.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.Files.Common.Configuration;

public sealed class ProgramConfiguration
{
	public const string EnvironmentKey = "Environment";

	public static void ConfigureHost(IConfigurationBuilder builder, string[] args)
	{
		Preconditions.RequiresNotNull(builder, nameof(builder));
		Preconditions.RequiresNotNull(args, nameof(args));

		builder.SetBasePath(Assemblies.GetExecutingDirectory());

		builder.AddEnvironmentVariables();
		builder.AddCommandLine(args);
	}

	public static void ConfigureApp(
		IConfigurationBuilder builder,
		Assembly assembly,
		string environment,
		string[] args)
	{
		Preconditions.RequiresNotNull(builder, nameof(builder));
		Preconditions.RequiresNotNull(assembly, nameof(assembly));
		Preconditions.RequiresNotNull(environment, nameof(environment));
		Preconditions.RequiresNotNull(args, nameof(args));

		builder.SetBasePath(Assemblies.GetExecutingDirectory());

		builder
			.AddJsonFile(
				"common-settings.json",
				optional: true,
				reloadOnChange: false
			)
			.AddJsonFile(
				$"common-settings.{environment}.json",
				optional: true,
				reloadOnChange: false
			)
			.AddJsonFile(
				"app-settings.json",
				optional: true,
				reloadOnChange: false
			)
			.AddJsonFile(
				$"app-settings.{environment}.json",
				optional: true,
				reloadOnChange: false
			);

		if (IsDevelopment(environment))
		{
			builder.AddUserSecrets(assembly, optional: true);
		}

		builder.AddEnvironmentVariables();

		builder.AddCommandLine(args);
	}

	private static bool IsDevelopment(string? environment)
		=> string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase);

	public static void ConfigureLogging(ILoggingBuilder builder, IConfiguration configuration, string environment)
	{
		Preconditions.RequiresNotNull(builder, nameof(builder));
		Preconditions.RequiresNotNull(configuration, nameof(configuration));
		Preconditions.RequiresNotNull(environment, nameof(environment));

		builder.AddConfiguration(configuration.GetSection("Logging"));
		if (IsDevelopment(environment))
		{
			builder.AddConsole();
			builder.AddDebug();
		}

		builder.Configure(
			options =>
			{
				options.ActivityTrackingOptions
					= ActivityTrackingOptions.SpanId
					  | ActivityTrackingOptions.TraceId
					  | ActivityTrackingOptions.ParentId;
			}
		);
	}

	public static ServiceProviderOptions ConfigureServiceProvider(string environment)
	{
		var options = new ServiceProviderOptions();
		ConfigureServiceProvider(options, environment);
		return options;
	}

	public static void ConfigureServiceProvider(ServiceProviderOptions options, string environment)
	{
		Preconditions.RequiresNotNull(options, nameof(options));

		if (IsDevelopment(environment))
		{
			options.ValidateScopes = true;
			options.ValidateOnBuild = true;
		}
	}

	public static ProgramConfiguration Create(Assembly assembly, string[] args)
	{
		Preconditions.RequiresNotNull(assembly, nameof(assembly));
		Preconditions.RequiresNotNull(args, nameof(args));

		var hostConfigurationBuilder = new ConfigurationBuilder();
		ConfigureHost(hostConfigurationBuilder, args);
		var hostConfiguration = hostConfigurationBuilder.Build();

		var environment = hostConfiguration.GetValue<string>(EnvironmentKey);

		var appConfigurationBuilder = new ConfigurationBuilder();
		ConfigureApp(
			appConfigurationBuilder,
			assembly,
			environment,
			args
		);
		var appConfiguration = appConfigurationBuilder.Build();

		return new ProgramConfiguration(hostConfiguration, appConfiguration);
	}

	private ProgramConfiguration(IConfigurationRoot host, IConfigurationRoot app)
	{
		Preconditions.RequiresNotNull(host, nameof(host));
		Preconditions.RequiresNotNull(app, nameof(app));

		Host = host;
		App = app;
	}

	public IConfigurationRoot Host { get; }
	public IConfigurationRoot App { get; }
}