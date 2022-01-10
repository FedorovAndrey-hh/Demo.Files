using Common.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.Files.Authorization.Presentations.WebApi.Utility.Behavior;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class IfNoneMatchUserVersionAttribute
	: Attribute,
	  IBinderTypeProviderMetadata,
	  IModelNameProvider
{
	public BindingSource BindingSource => BindingSource.Header;

	public Type BinderType { get; } = typeof(UserVersionFromEntityTagBinder);

	public string Name { get; set; } = HttpHeaderConstants.IfNoneMatch;
}