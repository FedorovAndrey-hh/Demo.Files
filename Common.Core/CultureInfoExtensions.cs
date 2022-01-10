using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Common.Core;

public static class CultureInfoExtensions
{
	[return: NotNullIfNotNull("this")]
	public static CultureInfo? AsReadOnly(this CultureInfo? @this)
		=> @this is null ? null : CultureInfo.ReadOnly(@this);
}