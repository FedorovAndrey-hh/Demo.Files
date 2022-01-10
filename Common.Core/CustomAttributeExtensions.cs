using System.Reflection;
using Common.Core.Diagnostics;

namespace Common.Core;

public static class CustomAttributeExtensions
{
	public static bool HasCustomAttribute<T>(this MemberInfo @this, bool inherit = true)
		where T : Attribute
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return Attribute.IsDefined(@this, typeof(T), inherit);
	}

	public static bool HasCustomAttribute<T>(this ParameterInfo @this, bool inherit = true)
		where T : Attribute
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return Attribute.IsDefined(@this, typeof(T), inherit);
	}
}