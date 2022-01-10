using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Common.Core.Diagnostics;

public static class Contracts
{
	[ContractAnnotation("condition: false => nothing")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, string? description)
	{
		if (!condition)
		{
			throw new ContractException(description);
		}
	}

	[ContractAnnotation("target: null => nothing; target: notnull => notnull")]
	[return: System.Diagnostics.CodeAnalysis.NotNull, NotNullIfNotNull("target")]
	public static T NotNull<T>([NoEnumeration] T? target, string? name)
	{
		Assert(target is not null, name is null ? "Value can not be null." : $"`{name}` can not be null.");

		return target;
	}

	[ContractAnnotation("target: null => nothing; target: notnull => notnull")]
	[return: System.Diagnostics.CodeAnalysis.NotNull, NotNullIfNotNull("target")]
	public static T NotNullReturnFrom<T>([NoEnumeration] T? target, string? methodName)
	{
		Assert(
			target is not null,
			methodName is null ? "Returned value can not be null." : $"`{methodName}()` can not return null."
		);

		return target;
	}

	[ContractAnnotation("=> nothing")]
	[DoesNotReturn]
	public static Exception UnreachableThrow()
	{
		throw new ContractException("Unreachable code has benn reached.");
	}

	[ContractAnnotation("=> nothing")]
	[DoesNotReturn]
	public static T UnreachableReturn<T>()
	{
		throw new ContractException("Unreachable code has benn reached.");
	}
}