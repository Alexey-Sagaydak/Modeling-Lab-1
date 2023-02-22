using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class ChiSquare
    {
        public double chiExperimental { get; private set; }
        public double chiCritical { get; private set; }
       public bool Check(IGenerator generator)
       {
            if (generator.IntervalAmount < 4)
                throw new ArgumentException("Interval amount must be greater than 3");
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            chiExperimental = 0; 
            chiCritical = excel.WorksheetFunction.ChiInv(0.05, generator.IntervalAmount - 2 - 1);

            for (int i = 0; i < generator.IntervalAmount; i++)
            {
                chiExperimental += Math.Pow(generator.Intervals[i].PointsAmount - generator.NumberAmount / generator.IntervalAmount, 2) 
                    / (generator.NumberAmount / generator.IntervalAmount); 
            }

            excel.Quit();
            return (chiExperimental < chiCritical) ? true : false;
       }
    }
}
