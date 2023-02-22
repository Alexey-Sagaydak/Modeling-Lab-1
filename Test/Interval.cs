using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public struct Interval
    {
        public double LeftBorder { get; set; }
        public double RightBorder { get; set; }
        public int PointsAmount { get; set; }
        public double Middle { get; set; }

        public Interval (double leftBorder, double rightBorder) : this()
        {
            if (rightBorder < leftBorder) throw new ArgumentException("Right border must be greater than the left one");
            PointsAmount = 0;
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            Middle = (RightBorder + LeftBorder) / 2.0;
        }
    }
}
