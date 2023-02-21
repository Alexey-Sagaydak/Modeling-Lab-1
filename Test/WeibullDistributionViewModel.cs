using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class WeibullDistributionViewModel
    {
        private IGenerator generator;
        private ChiSquare chiSquare;

        public double k { get; set; }
        public double lambda { get; set; }
        public int numberAmount { get; set; }
        public int intervalAmount { get; set; }
        public double[] Frequencies { get; private set; }
        public double[] CurvePoints { get; private set; }
        public double chiExperimental { get; private set; }
        public double chiCritical { get; private set; }
        public bool chiSquareResult { get; private set; }
        
        public double[] Middles { get; private set; }
        private void InitializeFields(double _k, double _lambda, int _numberAmount, int _interalAmount)
        {
            if (_k < 0) throw new ArgumentOutOfRangeException(nameof(_k));
            if (_lambda < 0) throw new ArgumentOutOfRangeException(nameof(_lambda));
            if (_numberAmount <= 0) throw new ArgumentOutOfRangeException(nameof(_numberAmount));
            if (_interalAmount <= 0) throw new ArgumentOutOfRangeException(nameof(_interalAmount));

            k = _k;
            lambda = _lambda;
            numberAmount = _numberAmount;
            intervalAmount = _interalAmount;

            Frequencies = new double[intervalAmount];
            CurvePoints = new double[intervalAmount * 10];
            Middles = new double[intervalAmount];

            generator = new PiecewiseApproximation(new WeibullDistribution(k, lambda), numberAmount, intervalAmount);
            chiSquare = new ChiSquare();
        }
        
        public void Calculate(double _k, double _lambda, int _numberAmount, int _interalAmount)
        {
            InitializeFields(_k, _lambda, _numberAmount, _interalAmount);
            generator.Generate();

            chiExperimental = chiSquare.chiExperimental;
            chiCritical = chiSquare.chiCritical;
            chiSquareResult = chiSquare.Check(generator);

            CalculateFrequencies();
            CalculateCurvePoints();
        }

        private void CalculateFrequencies()
        {
            for (int i = 0; i < intervalAmount; i++)
            {
                Frequencies[i] = (double)generator.Intervals[i].PointsAmount / (double)numberAmount;
                Middles[i] = generator.Intervals[i].Middle;
            }

        }
        private void CalculateCurvePoints()
        {
            int index = 0;
            for (double i = 0; i < generator.Intervals[intervalAmount - 1].RightBorder; i += 0.1)
            {
                CurvePoints[index] = generator.weibullDistribution.Calculate(i);
                index++;
            }
        }
    }
}
