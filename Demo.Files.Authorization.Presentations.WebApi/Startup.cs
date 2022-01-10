using Common.Core.Diagnostics;
using Common.Web;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Authorization.Presentation.Common;
using Demo.Files.Authorization.Presentations.WebApi.Utility.Authentication;
using Demo.Files.Authorization.Presentations.WebApi.Utility.Behavior;
using Demo.Files.Common.Contracts;
using Demo.Files.Common.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;

namespace Demo.Files.Authorization.Presentations.WebApi;

public sealed class Startup
{
	public Startup(IConfiguration configuration)
	{
		Preconditions.RequiresNotNull(configuration, nameof(configuration));

		_configuration = configuration;
	}

	private readonly IConfiguration _configuration;

	public void ConfigureServices(IServiceCollection services)
	{
		Preconditions.RequiresNotNull(services, nameof(services));

		services.AddCors();

		services.AddFilesProviderAuthentication(_configuration);
		services.AddAuthorization();

		services
			.AddControllers(
				options =>
				{
					options.SuppressAsyncSuffixInActionNames = false;

					options.ModelBinderProviders.RemoveType<EnumTypeModelBinderProvider>();
					options.ModelBinderProviders.Insert(0, new ModelBinderProvider());

					options.Filters.Add(new DomainValidationExceptionFilter());
					options.Filters.Add(new InternalServerExceptionFilter());

					options.AddResponseStatusCodes(
						StatusCodes.Status401Unauthorized,
						StatusCodes.Status403Forbidden,
						StatusCodes.Status404NotFound
					);
					options.AddResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest);
					options.AddResponseType<ValidationProblemDetails>(StatusCodes.Status409Conflict);
					options.AddResponseType<ValidationProblemDetails>(StatusCodes.Status422UnprocessableEntity);
					options.AddResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError);
				}
			)
			.ConfigureApiBehaviorOptions(
				options =>
				{
					options.InvalidModelStateResponseFactory = context =>
					{
						var details = new ValidationProblemDetails(context.ModelState);
						details.SetTraceId(context.HttpContext);
						return new UnprocessableEntityObjectResult(details);
					};
				}
			)
			.AddJsonOptions(options => { Serialization.Configure(options.JsonSerializerOptions); });

		services.AddUseCasesWithDomainImplementation(
			_configuration.GetSection("PersistenceOptions"),
			_configuration.GetSection("CommunicationOptions")
		);

		services.AddFilesDocumentation(
			(settings, _) =>
			{
				settings.Title = "Files authorization API docs";

				settings.TypeMappers.Add(
					new PrimitiveTypeMapper(
						typeof(UserVersion),
						schema => schema.Type = JsonObjectType.Integer
					)
				);
			}
		);
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		Preconditions.RequiresNotNull(app, nameof(app));

		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();
		app.UseRequestLocalization(
			options =>
			{
				options.SetDefaultCulture("en");
				options.AddSupportedCultures("en");
				options.AddSupportedUICultures("en");
			}
		);
		app.UseFilesDocumentation(RouteContextNames.Authorization);

		app.UseFilesProviderAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
	}
}