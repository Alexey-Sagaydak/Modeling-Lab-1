using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Test
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            PiecewiseApproximation piecewiseApproximation = new PiecewiseApproximation(new WeibullDistribution(2, 1.5), 1000, 60);
            piecewiseApproximation.Generate();
            Console.WriteLine(ChiSquare.Check(piecewiseApproximation));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
