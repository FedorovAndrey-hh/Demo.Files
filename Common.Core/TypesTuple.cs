namespace Common.Core;

public static class TypesTuple
{
	public static (Type, Type) Of<T1, T2>() => (typeof(T1), typeof(T2));

	public static (Type, Type) OfSubtypes<T1, T2>()
		where T2 : T1
		=> (typeof(T1), typeof(T2));
}