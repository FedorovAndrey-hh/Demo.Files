using Common.Core.Diagnostics;
using Common.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.Files.Query.WebApi.Utility.Behavior;

public sealed class ModelBinderProvider : IModelBinderProvider
{
	public IModelBinder? GetBinder(ModelBinderProviderContext context)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		var modelType = context.Metadata.UnderlyingOrModelType;

		return modelType switch
		{
			_ when modelType.IsEnum => new EnumAsStringBinder(modelType),
			_ => null
		};
	}
}