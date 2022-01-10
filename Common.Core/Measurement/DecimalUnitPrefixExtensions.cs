using Common.Core.Diagnostics;

namespace Common.Core.Measurement;

public static class DecimalUnitPrefixExtensions
{
	public static TValue ToYotta<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Yotta);
	}

	public static double ToYotta(this DecimalUnitPrefix @this, double value)
		=> @this.ToYotta(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToZetta<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Zetta);
	}

	public static double ToZetta(this DecimalUnitPrefix @this, double value)
		=> @this.ToZetta(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToExa<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Exa);
	}

	public static double ToExa(this DecimalUnitPrefix @this, double value)
		=> @this.ToExa(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToPeta<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Peta);
	}

	public static double ToPeta(this DecimalUnitPrefix @this, double value)
		=> @this.ToPeta(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToTera<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Tera);
	}

	public static double ToTera(this DecimalUnitPrefix @this, double value)
		=> @this.ToTera(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToGiga<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Giga);
	}

	public static double ToGiga(this DecimalUnitPrefix @this, double value)
		=> @this.ToGiga(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToMega<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Mega);
	}

	public static double ToMega(this DecimalUnitPrefix @this, double value)
		=> @this.ToMega(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToKilo<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Kilo);
	}

	public static double ToKilo(this DecimalUnitPrefix @this, double value)
		=> @this.ToKilo(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToHecto<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Hecto);
	}

	public static double ToHecto(this DecimalUnitPrefix @this, double value)
		=> @this.ToHecto(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToDeca<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Deca);
	}

	public static double ToDeca(this DecimalUnitPrefix @this, double value)
		=> @this.ToDeca(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToNone<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.None);
	}

	public static double ToNone(this DecimalUnitPrefix @this, double value)
		=> @this.ToNone(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToDeci<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Deci);
	}

	public static double ToDeci(this DecimalUnitPrefix @this, double value)
		=> @this.ToDeci(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToCenti<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Centi);
	}

	public static double ToCenti(this DecimalUnitPrefix @this, double value)
		=> @this.ToCenti(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToMilli<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Milli);
	}

	public static double ToMilli(this DecimalUnitPrefix @this, double value)
		=> @this.ToMilli(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToMicro<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Micro);
	}

	public static double ToMicro(this DecimalUnitPrefix @this, double value)
		=> @this.ToMicro(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToNano<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Nano);
	}

	public static double ToNano(this DecimalUnitPrefix @this, double value)
		=> @this.ToNano(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToPico<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Pico);
	}

	public static double ToPico(this DecimalUnitPrefix @this, double value)
		=> @this.ToPico(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToFemto<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Femto);
	}

	public static double ToFemto(this DecimalUnitPrefix @this, double value)
		=> @this.ToFemto(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToAtto<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Atto);
	}

	public static double ToAtto(this DecimalUnitPrefix @this, double value)
		=> @this.ToAtto(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToZepto<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Zepto);
	}

	public static double ToZepto(this DecimalUnitPrefix @this, double value)
		=> @this.ToZepto(value, DecimalUnitPrefixMeasurers.Create().Double);

	public static TValue ToYocto<TValue>(
		this DecimalUnitPrefix @this,
		TValue value,
		IMeasurer<TValue, DecimalUnitPrefix> measurer)
		where TValue : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(value, nameof(value));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		return measurer.Measure(value, @this, DecimalUnitPrefix.Yocto);
	}

	public static double ToYocto(this DecimalUnitPrefix @this, double value)
		=> @this.ToYocto(value, DecimalUnitPrefixMeasurers.Create().Double);
}