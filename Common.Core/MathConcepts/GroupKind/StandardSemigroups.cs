namespace Common.Core.MathConcepts.GroupKind;

public static class StandardSemigroups
{
	public static ISemigroup<int> IntAddition => StandardMonoids.IntAddition;
	public static ISemigroup<int> IntMultiplication => StandardMonoids.IntMultiplication;

	public static ISemigroup<uint> UintAddition => StandardMonoids.UintAddition;
	public static ISemigroup<uint> UintMultiplication => StandardMonoids.UintMultiplication;

	public static ISemigroup<long> LongAddition => StandardMonoids.LongAddition;
	public static ISemigroup<long> LongMultiplication => StandardMonoids.LongMultiplication;

	public static ISemigroup<ulong> UlongAddition => StandardMonoids.UlongAddition;
	public static ISemigroup<ulong> UlongMultiplication => StandardMonoids.UlongMultiplication;

	public static ISemigroup<double> DoubleAddition => StandardGroups.DoubleAddition;
	public static ISemigroup<double> DoubleMultiplication => StandardGroups.DoubleMultiplication;

	public static ISemigroup<float> FloatAddition => StandardGroups.FloatAddition;
	public static ISemigroup<float> FloatMultiplication => StandardGroups.FloatMultiplication;

	public static ISemigroup<decimal> DecimalAddition => StandardGroups.DecimalAddition;
	public static ISemigroup<decimal> DecimalMultiplication => StandardGroups.DecimalMultiplication;
}