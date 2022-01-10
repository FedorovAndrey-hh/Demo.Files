namespace Common.Core.Diagnostics;

public static class Errors
{
	public static EnvironmentException EnvironmentVariableNotFound(string name)
	{
		Preconditions.RequiresNotNull(name, nameof(name));

		return EnvironmentException.VariableNotFound(name);
	}

	public static FormatException InvalidFormat(string? format, Exception? cause)
		=> new($"The format of `{format}` is invalid.", cause);

	public static ContractException UnsupportedEnumValue<T>(T value)
		where T : struct, Enum
	{
		return new ContractException($"{Enum.GetName<T>(value)} is not supported.");
	}

	public static ContractException UnsupportedType(Type type)
	{
		Preconditions.RequiresNotNull(type, nameof(type));

		return new ContractException($"{type.FullName} is not supported.");
	}

	public static ContractException UnsupportedType<T>() => UnsupportedType(typeof(T));

	public static ContractException UnsupportedOperation(string name)
	{
		Preconditions.RequiresNotNull(name, nameof(name));

		return new ContractException($"{name} operation is not supported.");
	}

	public static ContractException UnsupportedEquality<T>()
		=> new($"{typeof(T).FullName} type does not support equality methods.");

	public static ContractException UnexpectedContract() => new("Type has unexpected interface.");
}