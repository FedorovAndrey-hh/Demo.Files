using Common.Core.Diagnostics;
using Common.Core.Execution;

namespace Common.Core;

public static class SwitchExtensions
{
	public static TResult Switch<TContext, TResult, TTypeEnum, TType1>(
		this ITypeEnum<TTypeEnum, TType1> @this,
		TContext context,
		Func<TContext, TType1, TResult> case1)
		where TType1 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(context, type1),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static TResult Switch<TResult, TTypeEnum, TType1>(
		this ITypeEnum<TTypeEnum, TType1> @this,
		Func<TType1, TResult> case1)
		where TType1 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(type1),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static Task<TResult> SwitchAsync<TContext, TResult, TTypeEnum, TType1>(
		this ITypeEnum<TTypeEnum, TType1> @this,
		TContext context,
		AsyncFunc<TContext, TType1, TResult> case1)
		where TType1 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(context, type1),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static Task<TResult> SwitchAsync<TResult, TTypeEnum, TType1>(
		this ITypeEnum<TTypeEnum, TType1> @this,
		AsyncFunc<TType1, TResult> case1)
		where TType1 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(type1),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static TResult Switch<TContext, TResult, TTypeEnum, TType1, TType2>(
		this ITypeEnum<TTypeEnum, TType1, TType2> @this,
		TContext context,
		Func<TContext, TType1, TResult> case1,
		Func<TContext, TType2, TResult> case2)
		where TType1 : TTypeEnum
		where TType2 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(context, type1),
			TType2 type2 => Preconditions.RequiresNotNull(case2, nameof(case2)).Invoke(context, type2),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static TResult Switch<TResult, TTypeEnum, TType1, TType2>(
		this ITypeEnum<TTypeEnum, TType1, TType2> @this,
		Func<TType1, TResult> case1,
		Func<TType2, TResult> case2)
		where TType1 : TTypeEnum
		where TType2 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(type1),
			TType2 type2 => Preconditions.RequiresNotNull(case2, nameof(case2)).Invoke(type2),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static Task<TResult> SwitchAsync<TContext, TResult, TTypeEnum, TType1, TType2>(
		this ITypeEnum<TTypeEnum, TType1, TType2> @this,
		TContext context,
		AsyncFunc<TContext, TType1, TResult> case1,
		AsyncFunc<TContext, TType2, TResult> case2)
		where TType1 : TTypeEnum
		where TType2 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(context, type1),
			TType2 type2 => Preconditions.RequiresNotNull(case2, nameof(case2)).Invoke(context, type2),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static Task<TResult> SwitchAsync<TResult, TTypeEnum, TType1, TType2>(
		this ITypeEnum<TTypeEnum, TType1, TType2> @this,
		AsyncFunc<TType1, TResult> case1,
		AsyncFunc<TType2, TResult> case2)
		where TType1 : TTypeEnum
		where TType2 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(type1),
			TType2 type2 => Preconditions.RequiresNotNull(case2, nameof(case2)).Invoke(type2),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static TResult Switch<TContext, TResult, TTypeEnum, TType1, TType2, TType3>(
		this ITypeEnum<TTypeEnum, TType1, TType2, TType3> @this,
		TContext context,
		Func<TContext, TType1, TResult> case1,
		Func<TContext, TType2, TResult> case2,
		Func<TContext, TType3, TResult> case3)
		where TType1 : TTypeEnum
		where TType2 : TTypeEnum
		where TType3 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(context, type1),
			TType2 type2 => Preconditions.RequiresNotNull(case2, nameof(case2)).Invoke(context, type2),
			TType3 type3 => Preconditions.RequiresNotNull(case3, nameof(case3)).Invoke(context, type3),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static TResult Switch<TResult, TTypeEnum, TType1, TType2, TType3>(
		this ITypeEnum<TTypeEnum, TType1, TType2, TType3> @this,
		Func<TType1, TResult> case1,
		Func<TType2, TResult> case2,
		Func<TType3, TResult> case3)
		where TType1 : TTypeEnum
		where TType2 : TTypeEnum
		where TType3 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(type1),
			TType2 type2 => Preconditions.RequiresNotNull(case2, nameof(case2)).Invoke(type2),
			TType3 type3 => Preconditions.RequiresNotNull(case3, nameof(case3)).Invoke(type3),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static Task<TResult> SwitchAsync<TContext, TResult, TTypeEnum, TType1, TType2, TType3>(
		this ITypeEnum<TTypeEnum, TType1, TType2, TType3> @this,
		TContext context,
		AsyncFunc<TContext, TType1, TResult> case1,
		AsyncFunc<TContext, TType2, TResult> case2,
		AsyncFunc<TContext, TType3, TResult> case3)
		where TType1 : TTypeEnum
		where TType2 : TTypeEnum
		where TType3 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(context, type1),
			TType2 type2 => Preconditions.RequiresNotNull(case2, nameof(case2)).Invoke(context, type2),
			TType3 type3 => Preconditions.RequiresNotNull(case3, nameof(case3)).Invoke(context, type3),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static Task<TResult> SwitchAsync<TResult, TTypeEnum, TType1, TType2, TType3>(
		this ITypeEnum<TTypeEnum, TType1, TType2, TType3> @this,
		AsyncFunc<TType1, TResult> case1,
		AsyncFunc<TType2, TResult> case2,
		AsyncFunc<TType3, TResult> case3)
		where TType1 : TTypeEnum
		where TType2 : TTypeEnum
		where TType3 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(type1),
			TType2 type2 => Preconditions.RequiresNotNull(case2, nameof(case2)).Invoke(type2),
			TType3 type3 => Preconditions.RequiresNotNull(case3, nameof(case3)).Invoke(type3),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static TResult Switch<TContext, TResult, TTypeEnum, TType1, TType2, TType3, TType4>(
		this ITypeEnum<TTypeEnum, TType1, TType2, TType3, TType4> @this,
		TContext context,
		Func<TContext, TType1, TResult> case1,
		Func<TContext, TType2, TResult> case2,
		Func<TContext, TType3, TResult> case3,
		Func<TContext, TType4, TResult> case4)
		where TType1 : TTypeEnum
		where TType2 : TTypeEnum
		where TType3 : TTypeEnum
		where TType4 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(context, type1),
			TType2 type2 => Preconditions.RequiresNotNull(case2, nameof(case2)).Invoke(context, type2),
			TType3 type3 => Preconditions.RequiresNotNull(case3, nameof(case3)).Invoke(context, type3),
			TType4 type4 => Preconditions.RequiresNotNull(case4, nameof(case4)).Invoke(context, type4),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static TResult Switch<TResult, TTypeEnum, TType1, TType2, TType3, TType4>(
		this ITypeEnum<TTypeEnum, TType1, TType2, TType3, TType4> @this,
		Func<TType1, TResult> case1,
		Func<TType2, TResult> case2,
		Func<TType3, TResult> case3,
		Func<TType4, TResult> case4)
		where TType1 : TTypeEnum
		where TType2 : TTypeEnum
		where TType3 : TTypeEnum
		where TType4 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(type1),
			TType2 type2 => Preconditions.RequiresNotNull(case2, nameof(case2)).Invoke(type2),
			TType3 type3 => Preconditions.RequiresNotNull(case3, nameof(case3)).Invoke(type3),
			TType4 type4 => Preconditions.RequiresNotNull(case4, nameof(case4)).Invoke(type4),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static Task<TResult> SwitchAsync<TContext, TResult, TTypeEnum, TType1, TType2, TType3, TType4>(
		this ITypeEnum<TTypeEnum, TType1, TType2, TType3, TType4> @this,
		TContext context,
		AsyncFunc<TContext, TType1, TResult> case1,
		AsyncFunc<TContext, TType2, TResult> case2,
		AsyncFunc<TContext, TType3, TResult> case3,
		AsyncFunc<TContext, TType4, TResult> case4)
		where TType1 : TTypeEnum
		where TType2 : TTypeEnum
		where TType3 : TTypeEnum
		where TType4 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(context, type1),
			TType2 type2 => Preconditions.RequiresNotNull(case2, nameof(case2)).Invoke(context, type2),
			TType3 type3 => Preconditions.RequiresNotNull(case3, nameof(case3)).Invoke(context, type3),
			TType4 type4 => Preconditions.RequiresNotNull(case4, nameof(case4)).Invoke(context, type4),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}

	public static Task<TResult> SwitchAsync<TResult, TTypeEnum, TType1, TType2, TType3, TType4>(
		this ITypeEnum<TTypeEnum, TType1, TType2, TType3, TType4> @this,
		AsyncFunc<TType1, TResult> case1,
		AsyncFunc<TType2, TResult> case2,
		AsyncFunc<TType3, TResult> case3,
		AsyncFunc<TType4, TResult> case4)
		where TType1 : TTypeEnum
		where TType2 : TTypeEnum
		where TType3 : TTypeEnum
		where TType4 : TTypeEnum
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this switch
		{
			TType1 type1 => Preconditions.RequiresNotNull(case1, nameof(case1)).Invoke(type1),
			TType2 type2 => Preconditions.RequiresNotNull(case2, nameof(case2)).Invoke(type2),
			TType3 type3 => Preconditions.RequiresNotNull(case3, nameof(case3)).Invoke(type3),
			TType4 type4 => Preconditions.RequiresNotNull(case4, nameof(case4)).Invoke(type4),
			_ => throw Errors.UnsupportedType(@this.GetType())
		};
	}
}