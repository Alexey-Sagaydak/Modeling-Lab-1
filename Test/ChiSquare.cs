using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public static class ChiSquare
    {
       public static bool Check(IGenerator generator)
       {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
             
            double chiCritical = excel.WorksheetFunction.ChiInv(0.05, generator.IntervalAmount - 2 - 1);
            double chiExperimental = 0;

            for (int i = 0; i < generator.IntervalAmount; i++)
            {
                chiExperimental += Math.Pow(generator.Intervals[i].PointsAmount - generator.NumberAmount / generator.IntervalAmount, 2) 
                    / (generator.NumberAmount / generator.IntervalAmount); 
            }
            Console.WriteLine($"{chiCritical} {chiExperimental}");
            excel.Quit();
            return (chiExperimental < chiCritical) ? true : false;
       }
    }
}
