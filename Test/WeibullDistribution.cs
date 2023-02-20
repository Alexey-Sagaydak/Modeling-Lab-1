using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class WeibullDistribution
    {
        public double K { get; set; }
        public double Lambda { get; set; }

        public WeibullDistribution(double k, double lambda)
        {
            if (k <= 0) throw new ArgumentOutOfRangeException(nameof(k));
            K = k;

            if (lambda <= 0) throw new ArgumentOutOfRangeException(nameof(lambda));
            Lambda = lambda;
        }

        public double Calculate(double x) => Lambda * K * Math.Pow(x, K - 1)
            * Math.Exp(-Lambda * Math.Pow(x, K));
    }
}
