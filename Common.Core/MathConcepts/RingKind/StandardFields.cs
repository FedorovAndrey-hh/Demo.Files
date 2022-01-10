namespace Common.Core.MathConcepts.RingKind;

public static class StandardFields
{
	public static IField<float> Float { get; } = new FloatField();

	private sealed class FloatField : IField<float>
	{
		private sealed class Addition
			: IField<float>.IAdditionOperation,
			  IField<float>.IAdditionInverseOperation
		{
			public float Apply(float left, float right) => left + right;

			public float Apply(float element) => -element;
		}

		private sealed class Multiplication
			: IField<float>.IMultiplicationOperation,
			  IField<float>.IMultiplicativeInverseOperation
		{
			public float Apply(float left, float right) => left * right;

			public float Apply(float element) => 1f / element;
		}

		private readonly Addition _addition = new();
		private readonly Multiplication _multiplication = new();

		public IField<float>.IAdditionOperation AdditionOperation => _addition;
		public IField<float>.IAdditionInverseOperation AdditiveInverseOperation => _addition;
		public float AdditiveIdentity => 0f;
		public IField<float>.IMultiplicationOperation MultiplicationOperation => _multiplication;
		public IField<float>.IMultiplicativeInverseOperation MultiplicativeInverseOperation => _multiplication;
		public float MultiplicativeIdentity => 1f;
	}

	public static IField<double> Double { get; } = new DoubleField();

	private sealed class DoubleField : IField<double>
	{
		private sealed class Addition
			: IField<double>.IAdditionOperation,
			  IField<double>.IAdditionInverseOperation
		{
			public double Apply(double left, double right) => left + right;

			public double Apply(double element) => -element;
		}

		private sealed class Multiplication
			: IField<double>.IMultiplicationOperation,
			  IField<double>.IMultiplicativeInverseOperation
		{
			public double Apply(double left, double right) => left * right;

			public double Apply(double element) => 1d / element;
		}

		private readonly Addition _addition = new();
		private readonly Multiplication _multiplication = new();

		public IField<double>.IAdditionOperation AdditionOperation => _addition;
		public IField<double>.IAdditionInverseOperation AdditiveInverseOperation => _addition;
		public double AdditiveIdentity => 0d;
		public IField<double>.IMultiplicationOperation MultiplicationOperation => _multiplication;
		public IField<double>.IMultiplicativeInverseOperation MultiplicativeInverseOperation => _multiplication;
		public double MultiplicativeIdentity => 1d;
	}

	public static IField<decimal> Decimal { get; } = new DecimalField();

	private sealed class DecimalField : IField<decimal>
	{
		private sealed class Addition
			: IField<decimal>.IAdditionOperation,
			  IField<decimal>.IAdditionInverseOperation
		{
			public decimal Apply(decimal left, decimal right) => left + right;

			public decimal Apply(decimal element) => -element;
		}

		private sealed class Multiplication
			: IField<decimal>.IMultiplicationOperation,
			  IField<decimal>.IMultiplicativeInverseOperation
		{
			public decimal Apply(decimal left, decimal right) => left * right;

			public decimal Apply(decimal element) => 1m / element;
		}

		private readonly Addition _addition = new();
		private readonly Multiplication _multiplication = new();

		public IField<decimal>.IAdditionOperation AdditionOperation => _addition;
		public IField<decimal>.IAdditionInverseOperation AdditiveInverseOperation => _addition;
		public decimal AdditiveIdentity => 0m;
		public IField<decimal>.IMultiplicationOperation MultiplicationOperation => _multiplication;
		public IField<decimal>.IMultiplicativeInverseOperation MultiplicativeInverseOperation => _multiplication;
		public decimal MultiplicativeIdentity => 1m;
	}
}