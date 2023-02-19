using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class ChiSquare
    {
        private readonly int numberAmount; 
        // M
        private readonly double k = 0.5;
        private readonly int intervalAmount = 20;
        // sigma
        private readonly double lambda = 0.16;

        public ChiSquare(int _numberAmount, double _k, int _intervalAmount, double _lambda) {
            if ( _numberAmount <= 0) throw new ArgumentOutOfRangeException("Number amount must be positive");
            numberAmount = _numberAmount;
            if (_intervalAmount <= 0) throw new ArgumentOutOfRangeException("Interval amount must be positive");
            intervalAmount = _intervalAmount;
            
            k = _k;
            lambda = _lambda;
        }
        


    }
}
