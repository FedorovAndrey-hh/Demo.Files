using Common.Core.Diagnostics;
using Common.Web;
using Demo.Files.Common.Configuration;
using Demo.Files.Common.Contracts;
using Demo.Files.Common.Web;
using Demo.Files.Query.Views.MongoDb;
using Demo.Files.Query.WebApi.Utility.Behavior;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Demo.Files.Query.WebApi;

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

		services.AddFilesConsumerAuthentication(_configuration);
		services.AddAuthorization();

		services
			.AddControllers(
				options =>
				{
					options.SuppressAsyncSuffixInActionNames = false;

					options.ModelBinderProviders.RemoveType<EnumTypeModelBinderProvider>();
					options.ModelBinderProviders.Insert(0, new ModelBinderProvider());

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

		services.AddFilesDocumentation((settings, _) => settings.Title = "Files query API docs");

		services.AddMongoDbFilesViews(
			_configuration.GetSection("PersistenceOptions"),
			options => options.Json = SerializationConfiguration.ConfigureJson()
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
		app.UseFilesDocumentation(RouteContextNames.Query);

		app.UseFilesConsumerAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
	}
}