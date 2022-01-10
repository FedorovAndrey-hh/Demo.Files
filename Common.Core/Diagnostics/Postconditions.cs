using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Common.Core.Diagnostics;

public static class Postconditions
{
	[ContractAnnotation("value: null => nothing; value: notnull => notnull")]
	[return: System.Diagnostics.CodeAnalysis.NotNull, NotNullIfNotNull("value")]
	public static T ReturnsNotNull<T>([NoEnumeration] T? value, string? description = null)
	{
		if (value is null)
		{
			throw new PostconditionException(description);
		}

		return value;
	}
}