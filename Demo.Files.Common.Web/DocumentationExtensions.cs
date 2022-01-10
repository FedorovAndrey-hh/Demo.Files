using Common.Core.Diagnostics;
using Demo.Files.Common.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace Demo.Files.Common.Web;

public static class DocumentationExtensions
{
	public static void AddFilesDocumentation(
		this IServiceCollection @this,
		Action<AspNetCoreOpenApiDocumentGeneratorSettings, IServiceProvider>? configure = null)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.AddSwaggerDocument(
			(settings, services) =>
			{
				if (configure is not null)
				{
					configure(settings, services);
				}

				settings.FlattenInheritanceHierarchy = true;

				settings.AddSecurity(
					"Bearer",
					new OpenApiSecurityScheme
					{
						Scheme = JwtBearerDefaults.AuthenticationScheme,
						Type = OpenApiSecuritySchemeType.ApiKey,
						Name = "Authorization",
						BearerFormat = "JWT",
						In = OpenApiSecurityApiKeyLocation.Header
					}
				);

				settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));

				settings.TypeMappers.Add(
					new PrimitiveTypeMapper(
						typeof(AsyncOperationStatus),
						schema =>
						{
							schema.Type = JsonObjectType.Integer;
							schema.Maximum = long.MaxValue;
							foreach (var name in Enum.GetNames<AsyncOperationStatus>())
							{
								schema.Enumeration.Add(name);
							}
						}
					)
				);
			}
		);
	}

	public static void UseFilesDocumentation(
		this IApplicationBuilder @this,
		string contextName,
		Action<SwaggerUi3Settings>? configure = null)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(contextName, nameof(contextName));

		var docsPath
			= $"/docs/{contextName}/{{documentName}}/{contextName}-open-api.json";
		@this.UseOpenApi(settings => { settings.Path = docsPath; });
		@this.UseSwaggerUi3(
			settings =>
			{
				if (configure is not null)
				{
					configure(settings);
				}

				settings.DocumentPath = docsPath;
				settings.Path = $"/docs/{contextName}";
				settings.EnableTryItOut = true;
				settings.DefaultModelsExpandDepth = 1;
				settings.AdditionalSettings["showCommonExtensions"] = true;
				settings.AdditionalSettings["displayRequestDuration"] = true;
				settings.AdditionalSettings["persistAuthorization"] = true;
			}
		);
	}
}