using Common.Core.Diagnostics;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Common.Web;

public class EnumAsStringBinder: IModelBinder
{
	public EnumAsStringBinder(Type modelType)
	{
		Preconditions.RequiresNotNull(modelType, nameof(modelType));

		_modelType = modelType;
	}

	private readonly Type _modelType;

	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		Preconditions.RequiresNotNull(bindingContext, nameof(bindingContext));

		var modelName = bindingContext.ModelName;

		var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
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

		if (char.IsDigit(value[0])
		    || !Enum.TryParse(
			    _modelType,
			    value,
			    ignoreCase: true,
			    out var result
		    ))
		{
			bindingContext.ModelState.TryAddModelError(
				modelName,
				string.Format(Resources.Error_ExpectEnumValue_Format, _modelType.Name)
			);

			return Task.CompletedTask;
		}

		bindingContext.Result = ModelBindingResult.Success(result);

		return Task.CompletedTask;
	}
}