using Common.Core.Diagnostics;

namespace Common.Core;

public static class BiConverterExtensions
{
	public static T2 ConvertForward<T1, T2>(this IBiConverter<T1, T2> @this, T1 value)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Forward.Convert(value);
	}

	public static T1 ConvertBackward<T1, T2>(this IBiConverter<T1, T2> @this, T2 value)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Backward.Convert(value);
	}
}