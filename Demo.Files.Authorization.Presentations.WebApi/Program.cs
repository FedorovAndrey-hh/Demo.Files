using Common.Core;
using Common.Core.Diagnostics;
using Common.DependencyInjection;
using Demo.Files.Authorization.Presentations.WebApi.Utility;
using Demo.Files.Common.Configuration;

namespace Demo.Files.Authorization.Presentations.WebApi;

internal static class Program
{
	internal static async Task Main(params string[] args)
	{
		Preconditions.RequiresNotNull(args, nameof(args));

		var host = _CreateHost(args);

		var env = host.Services.GetRequiredService<IHostEnvironment>();
		if (env.IsDevelopment())
		{
			await DebugPlugin.HandleDebugOptionsAsync(host.Services).ConfigureAwait(false);
		}

		await host.Services.WaitForEnvironmentSetupAsync(Programs.CancelOrProcessExitCancellationToken())
			.ConfigureAwait(false);

		await host.RunAsync().ConfigureAwait(false);
	}

	private static IHost _CreateHost(string[] args)
	{
		Preconditions.RequiresNotNull(args, nameof(args));

		return new HostBuilder()
			.UseContentRoot(Directory.GetCurrentDirectory())
			.ConfigureHostConfiguration(builder => ProgramConfiguration.ConfigureHost(builder, args))
			.ConfigureAppConfiguration(
				(context, builder) => ProgramConfiguration.ConfigureApp(
					builder,
					typeof(Program).Assembly,
					context.HostingEnvironment.EnvironmentName,
					args
				)
			)
			.ConfigureLogging(
				(context, builder) =>
					ProgramConfiguration.ConfigureLogging(
						builder,
						context.Configuration,
						context.HostingEnvironment.EnvironmentName
					)
			)
			.ConfigureServices(
				(context, collection) =>
				{
					if (context.HostingEnvironment.IsDevelopment())
					{
						collection.Install<DebugPlugin>();
					}
				}
			)
			.UseDefaultServiceProvider(
				(context, options) => ProgramConfiguration.ConfigureServiceProvider(
					options,
					context.HostingEnvironment.EnvironmentName
				)
			)
			.ConfigureWebHost(
				builder => builder
					.UseStartup<Startup>()
					.UseKestrel()
					.UseIISIntegration()
			)
			.Build();
	}
}