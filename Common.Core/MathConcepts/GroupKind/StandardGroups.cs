using Common.Core.MathConcepts.Operations;

namespace Common.Core.MathConcepts.GroupKind;

public static class StandardGroups
{
	public static IGroup<int> IntAddition { get; } = new IntAdditionGroup();

	private class IntAdditionGroup
		: IGroup<int>,
		  ISemigroup<int>.IOperation,
		  IGroup<int>.IInverseOperation
	{
		public int Apply(int left, int right) => checked(left + right);

		public int Apply(int element) => checked(-element);

		public int Identity => 0;
		public ISemigroup<int>.IOperation Operation => this;
		public IGroup<int>.IInverseOperation InverseOperation => this;
	}

	public static IGroup<long> LongAddition { get; } = new LongAdditionGroup();

	private class LongAdditionGroup
		: IGroup<long>,
		  ISemigroup<long>.IOperation,
		  IGroup<long>.IInverseOperation
	{
		public long Apply(long left, long right) => checked(left + right);

		public long Apply(long element) => checked(-element);

		public long Identity => 0;
		public ISemigroup<long>.IOperation Operation => this;
		public IGroup<long>.IInverseOperation InverseOperation => this;
	}

	public static IGroup<float> FloatAddition { get; } = new FloatAdditionGroup();

	private sealed class FloatAdditionGroup
		: IGroup<float>,
		  ISemigroup<float>.IOperation,
		  IGroup<float>.IInverseOperation
	{
		float IBinaryOperation<float, float, float>.Apply(float left, float right) => left + right;
		float IUnaryOperation<float>.Apply(float element) => -element;

		public ISemigroup<float>.IOperation Operation => this;
		public float Identity => 0f;
		public IGroup<float>.IInverseOperation InverseOperation => this;
	}

	public static IGroup<float> FloatMultiplication { get; } = new FloatMultiplicationGroup();

	private sealed class FloatMultiplicationGroup
		: IGroup<float>,
		  ISemigroup<float>.IOperation,
		  IGroup<float>.IInverseOperation
	{
		float IBinaryOperation<float, float, float>.Apply(float left, float right) => left * right;
		float IUnaryOperation<float>.Apply(float element) => 1f / element;

		public ISemigroup<float>.IOperation Operation => this;
		public float Identity => 1f;
		public IGroup<float>.IInverseOperation InverseOperation => this;
	}

	public static IGroup<double> DoubleAddition { get; } = new DoubleAdditionGroup();

	private sealed class DoubleAdditionGroup
		: IGroup<double>,
		  ISemigroup<double>.IOperation,
		  IGroup<double>.IInverseOperation
	{
		double IBinaryOperation<double, double, double>.Apply(double left, double right) => left + right;
		double IUnaryOperation<double>.Apply(double element) => -element;

		public ISemigroup<double>.IOperation Operation => this;
		public double Identity => 0d;
		public IGroup<double>.IInverseOperation InverseOperation => this;
	}

	public static IGroup<double> DoubleMultiplication { get; } = new DoubleMultiplicationGroup();

	private sealed class DoubleMultiplicationGroup
		: IGroup<double>,
		  ISemigroup<double>.IOperation,
		  IGroup<double>.IInverseOperation
	{
		double IBinaryOperation<double, double, double>.Apply(double left, double right) => left * right;
		double IUnaryOperation<double>.Apply(double element) => 1d / element;

		public ISemigroup<double>.IOperation Operation => this;
		public double Identity => 1d;
		public IGroup<double>.IInverseOperation InverseOperation => this;
	}

	public static IGroup<decimal> DecimalAddition { get; } = new DecimalAdditionGroup();

	private sealed class DecimalAdditionGroup
		: IGroup<decimal>,
		  ISemigroup<decimal>.IOperation,
		  IGroup<decimal>.IInverseOperation
	{
		decimal IBinaryOperation<decimal, decimal, decimal>.Apply(decimal left, decimal right) => left + right;
		decimal IUnaryOperation<decimal>.Apply(decimal element) => -element;

		public ISemigroup<decimal>.IOperation Operation => this;
		public decimal Identity => 0m;
		public IGroup<decimal>.IInverseOperation InverseOperation => this;
	}

	public static IGroup<decimal> DecimalMultiplication { get; } = new DecimalMultiplicationGroup();

	private sealed class DecimalMultiplicationGroup
		: IGroup<decimal>,
		  ISemigroup<decimal>.IOperation,
		  IGroup<decimal>.IInverseOperation
	{
		decimal IBinaryOperation<decimal, decimal, decimal>.Apply(decimal left, decimal right) => left * right;
		decimal IUnaryOperation<decimal>.Apply(decimal element) => 1m / element;

		public ISemigroup<decimal>.IOperation Operation => this;
		public decimal Identity => 1m;
		public IGroup<decimal>.IInverseOperation InverseOperation => this;
	}
}