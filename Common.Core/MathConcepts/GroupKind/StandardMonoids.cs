namespace Common.Core.MathConcepts.GroupKind;

public static class StandardMonoids
{
	public static IMonoid<int> IntAddition => StandardGroups.IntAddition;

	public static IMonoid<int> IntMultiplication { get; } = new IntMultiplicationMonoid();

	private class IntMultiplicationMonoid
		: IMonoid<int>,
		  ISemigroup<int>.IOperation
	{
		public int Apply(int left, int right) => checked(left * right);

		public int Identity => 1;
		public ISemigroup<int>.IOperation Operation => this;
	}

	public static IMonoid<uint> UintAddition { get; } = new UintAdditionMonoid();

	private class UintAdditionMonoid
		: IMonoid<uint>,
		  ISemigroup<uint>.IOperation
	{
		public uint Apply(uint left, uint right) => checked(left + right);

		public uint Identity => 0u;
		public ISemigroup<uint>.IOperation Operation => this;
	}

	public static IMonoid<uint> UintMultiplication { get; } = new UintMultiplicationMonoid();

	private class UintMultiplicationMonoid
		: IMonoid<uint>,
		  ISemigroup<uint>.IOperation
	{
		public uint Apply(uint left, uint right) => checked(left * right);

		public uint Identity => 1u;
		public ISemigroup<uint>.IOperation Operation => this;
	}

	public static IMonoid<long> LongAddition => StandardGroups.LongAddition;

	public static IMonoid<long> LongMultiplication { get; } = new LongMultiplicationMonoid();

	private class LongMultiplicationMonoid
		: IMonoid<long>,
		  ISemigroup<long>.IOperation
	{
		public long Apply(long left, long right) => checked(left * right);

		public long Identity => 1L;
		public ISemigroup<long>.IOperation Operation => this;
	}

	public static IMonoid<ulong> UlongAddition { get; } = new UlongAdditionMonoid();

	private class UlongAdditionMonoid
		: IMonoid<ulong>,
		  ISemigroup<ulong>.IOperation
	{
		public ulong Apply(ulong left, ulong right) => checked(left + right);

		public ulong Identity => 0uL;
		public ISemigroup<ulong>.IOperation Operation => this;
	}

	public static IMonoid<ulong> UlongMultiplication { get; } = new UlongMultiplicationMonoid();

	private class UlongMultiplicationMonoid
		: IMonoid<ulong>,
		  ISemigroup<ulong>.IOperation
	{
		public ulong Apply(ulong left, ulong right) => checked(left * right);

		public ulong Identity => 1uL;
		public ISemigroup<ulong>.IOperation Operation => this;
	}
}