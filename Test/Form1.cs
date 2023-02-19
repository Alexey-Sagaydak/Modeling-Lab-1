using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private readonly int numberAmount = 1000;
        // M
        private readonly int intervalAmount = 20;
        // sigma
        private double lambda;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            // Массив значений выборки
            double[] x = new double[numberAmount];
            Random r = new Random();

            lambda = Convert.ToDouble(lambdaTextBox.Text);
            // Получение k из textBox1
            double k = Convert.ToDouble(kTextBox.Text);

            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();


            for (int i = 0; i < numberAmount; i++)
            {
                while (true)
                {
                    double randX = r.NextDouble() * 2.5;

                    double randY = r.NextDouble();
                    double theorY = lambda * k * Math.Pow(randX, k - 1) * Math.Exp(-lambda * Math.Pow(randX, k));

                    if (randY <= theorY)
                    {
                        x[i] = randX;
                        chart2.Series[0].Points.AddXY(randX, theorY);
                        chart2.Series[1].Points.AddXY(randX, randY);
                        break;
                    }
                }
            }

            double max = x.Max(), min = x.Min();

            // Длина интервала
            double intervalLen = (max - min) / intervalAmount;

            // Середины интервалов
            double[] middles = new double[intervalAmount];

            // Частоты попаданий значений в интервалы
            double[] frequencies = new double[intervalAmount];

            // Значения нормальной кривой
            double[] n = new double[intervalAmount];

            // Заполнение массивов середин интервалов и частот
            for (int i = 0; i < intervalAmount; i++)
            {
                int amountInInterval = 0;
                double leftBorder = min + intervalLen * i;
                double rightBorder = min + intervalLen * (i + 1);

                for (int j = 0; j < x.Length; j++)
                    if (x[j] >= leftBorder && x[j] < rightBorder) amountInInterval++;

                frequencies[i] = amountInInterval;
                middles[i] = (leftBorder + rightBorder) / 2;
            }

            // Выборочное среднее
            double sampleMean = getSampleMean(middles, frequencies);

            // Среднее квадратическое отклонение
            double standardDeviation = getDeviation(middles, frequencies, sampleMean);

            // Хи квадрат наблюдаемое, кол-во степеней свободы
            double chiSquareSeen = 0, degree = intervalAmount - 2 - 1;

            // Хи квадрат критическое
            double chiSquareCrit = excel.WorksheetFunction.ChiInv(0.05, degree);

            // Считаем точки для нормальной кривой, а также хи квадрат наблюдаемое
            for (int i = 0; i < intervalAmount; i++)
            {
                double theorY = lambda * k * Math.Pow((middles[i] - sampleMean) / standardDeviation, k - 1) 
                    * Math.Exp(-lambda * Math.Pow((middles[i] - sampleMean) / standardDeviation, k));
                Console.WriteLine(Math.Pow((middles[i] - sampleMean) / standardDeviation, k - 1));
                Console.WriteLine( Math.Exp(-lambda * Math.Pow((middles[i] - sampleMean) / standardDeviation, k)));
                // Теоретические частоты
                n[i] = intervalLen * numberAmount / standardDeviation * theorY;

                chiSquareSeen += Math.Pow(frequencies[i] - n[i], 2) / (n[i]);
            }

            // вывод данных на экран в поля textBox(2-5)
            chiSquareObservableTextBox.Text = chiSquareSeen.ToString();
            chiSquareCriticalTextBox.Text = chiSquareCrit.ToString();

            // Проверка критерия Пирсона
            if (chiSquareSeen > chiSquareCrit)
                label7.Text = "Критерий Пирсона не выполняется";
            else
                label7.Text = "Критерий Пирсона выполняется";

            excel.Quit();
            buildHist(middles, frequencies, n);
        }

        // ср. квадр. откл
        double getDeviation(double[] mid, double[] freq, double sampleMean)
        {
            double deviation = 0;

            for (int i = 0; i < mid.Length; i++)
                deviation += Math.Pow(mid[i], 2) * freq[i];
            
            deviation = Math.Sqrt(deviation / numberAmount - Math.Pow(sampleMean, 2));

            return deviation;
        }
        
        // выбор. среднее
        double getSampleMean(double[] mid, double[] freq) 
        {
            double sampleMean = 0;
            
            for (int i = 0; i < mid.Length; i++)
                sampleMean += mid[i]*freq[i];
            
            sampleMean /= numberAmount;
            return sampleMean;
        }

        // Построение гистограммы и нормальной кривой
        void buildHist(double[] middles, double[] histFreq, double[] normCurve)
        {
            // Очищаем гистограмму
            chart1.Series[0].Points.Clear(); 
            chart1.Series[1].Points.Clear();
            // Построение гистограммы
            for (int i = 0; i < intervalAmount; i++)
            {
                chart1.Series[0].Points.AddXY(middles[i], histFreq[i]);
                chart1.Series[1].Points.AddXY(middles[i], normCurve[i]);
            }
        }
    }
}
