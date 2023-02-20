using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class PiecewiseApproximation : IGenerator
    {
        private readonly Random random;
        public WeibullDistribution weibullDistribution { get; private set; } 
        public Interval[] Intervals { get; private set; }
        public int NumberAmount { get; set; }
        public int IntervalAmount { get; set; }

        private PiecewiseApproximation()
        {
            random = new Random();
        }

        public PiecewiseApproximation(WeibullDistribution _weibullDistribution, int numberAmount, int intervalAmount) : this()
        {

            if (numberAmount <= 0) throw new ArgumentOutOfRangeException(nameof(numberAmount));
            if (intervalAmount <= 0) throw new ArgumentOutOfRangeException(nameof(intervalAmount));
            if (_weibullDistribution == null) throw new ArgumentNullException(nameof(_weibullDistribution));
            weibullDistribution = _weibullDistribution;
            IntervalAmount = intervalAmount;
            NumberAmount = numberAmount;
            Intervals = new Interval[IntervalAmount];
            InitializeIntervalsArray();
        }

        public void Generate()
        {
            for (int i = 0; i < NumberAmount; i++)
            {
                int intervalIndex = (int)Math.Round((IntervalAmount - 1) * random.NextDouble());
                Intervals[intervalIndex].PointsAmount++;
            }
        }

        private void InitializeIntervalsArray()
        {
            Intervals[0] = new Interval(0, CalculateNextBorder(0));
            for (int i = 1; i < IntervalAmount; i++)
            {
                double leftBorder = Intervals[i - 1].RightBorder;
                Intervals[i] = new Interval(leftBorder, CalculateNextBorder(leftBorder));
            }
        }

        private double CalculateNextBorder(double leftBorder)
        {
            double t = Math.Log(Math.Exp(-weibullDistribution.Lambda * Math.Pow(leftBorder, weibullDistribution.K)) - 1.0 / (double) IntervalAmount) / -weibullDistribution.Lambda;
            return Math.Pow(t, (double)1 / weibullDistribution.K);
        }
    }
}
