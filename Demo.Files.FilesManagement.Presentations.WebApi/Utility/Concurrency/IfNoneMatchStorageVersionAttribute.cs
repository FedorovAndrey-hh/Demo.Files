using Common.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.Files.FilesManagement.Presentations.WebApi.Utility.Concurrency;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class IfNoneMatchStorageVersionAttribute
	: Attribute,
	  IBinderTypeProviderMetadata,
	  IModelNameProvider
{
	public BindingSource BindingSource => BindingSource.Header;

	public Type BinderType { get; } = typeof(StorageVersionFromEntityTagBinder);

	public string Name { get; set; } = HttpHeaderConstants.IfNoneMatch;
}