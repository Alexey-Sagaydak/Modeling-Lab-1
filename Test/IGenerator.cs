namespace Test
{
    public interface IGenerator
    {
        WeibullDistribution weibullDistribution { get; }
        int IntervalAmount { get; }
        Interval[] Intervals { get; }
        int NumberAmount { get; }

        void Generate();
    }
}