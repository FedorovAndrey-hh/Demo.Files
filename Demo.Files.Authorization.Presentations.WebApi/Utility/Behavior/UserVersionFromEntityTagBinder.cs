using Common.Core;
using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;

namespace Demo.Files.Authorization.Presentations.WebApi.Utility.Behavior;

public sealed class UserVersionFromEntityTagBinder : IModelBinder
{
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		Preconditions.RequiresNotNull(bindingContext, nameof(bindingContext));

		var modelName = bindingContext.ModelName;

		var valueProviderResult = bindingContext.HttpContext.Request.Headers.TryGetValue(modelName, out var raw)
			? new ValueProviderResult(raw)
			: ValueProviderResult.None;
		if (valueProviderResult == ValueProviderResult.None)
		{
			return Task.CompletedTask;
		}

		bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

		var value = valueProviderResult.FirstValue;

		if (string.IsNullOrEmpty(value))
		{
			return Task.CompletedTask;
		}

		UserVersion? userVersion;
		try
		{
			var entityTag = EntityTagHeaderValue.Parse(value);
			if (Eq.Value(entityTag, EntityTagHeaderValue.Any))
			{
				userVersion = null;
			}
			else
			{
				var tag = entityTag.Tag;
				userVersion = UserVersion.Of(ulong.Parse(tag.Substring(1, tag.Length - 2)));
			}
		}
		catch (Exception e)
		{
			bindingContext.ModelState.TryAddModelException(modelName, e);
			return Task.CompletedTask;
		}

		bindingContext.Result = ModelBindingResult.Success(userVersion);

		return Task.CompletedTask;
	}
}