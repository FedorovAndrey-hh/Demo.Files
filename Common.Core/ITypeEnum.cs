using System.Diagnostics.CodeAnalysis;

namespace Common.Core;

[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public interface ITypeEnum<TThis, TType1>
	where TType1 : TThis
{
}

[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public interface ITypeEnum<TThis, TType1, TType2>
	where TType1 : TThis
	where TType2 : TThis
{
}

[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public interface ITypeEnum<TThis, TType1, TType2, TType3>
	where TType1 : TThis
	where TType2 : TThis
	where TType3 : TThis
{
}

[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public interface ITypeEnum<TThis, TType1, TType2, TType3, TType4>
	where TType1 : TThis
	where TType2 : TThis
	where TType3 : TThis
	where TType4 : TThis
{
}