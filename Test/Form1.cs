using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        int N = 1000; // кол-во сгенерированных чисел
        double M = 0.5; // данные для интервала
        int INTERVAL_AMOUNT = 20;
        double SIGMA = 0.16;
        // k = 3 покрывает весь интервал 0 - 1
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            
            // Массив значений выборки
            double[] x = new double[N]; 
            Random r = new Random();

            // Получение k из textBox1
            double k = Convert.ToDouble(textBox1.Text);

            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();

            for (int i = 0; i < N; i++)
            {
                while (true)
                {
                    double randX = r.NextDouble();

                    if (randX < M - SIGMA * k || randX > M + SIGMA * k) continue;

                    double randY = r.NextDouble() * (2.5 - 0) + 0;
                    double theorY = Math.Exp(-(((randX - M) * (randX - M)) / (2 * SIGMA * SIGMA)))
                        / (SIGMA * Math.Sqrt(2 * Math.PI));

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
            double intervalLen = (max - min) / INTERVAL_AMOUNT;
            
            // Середины интервалов
            double[] middles = new double[INTERVAL_AMOUNT];

            // Частоты попаданий значений в интервалы
            double[] frequencies = new double[INTERVAL_AMOUNT];

            // Значения нормальной кривой
            double[] n = new double[INTERVAL_AMOUNT];

            // Заполнение массивов середин интервалов и частот
            for (int i = 0; i < INTERVAL_AMOUNT; i++) 
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
            double chiSquareSeen = 0, degree = INTERVAL_AMOUNT - 2 - 1;
            
            // Сумма частот
            double freqSum = excel.WorksheetFunction.Sum(frequencies);

            // Хи квадрат критическое
            double chiSquareCrit = excel.WorksheetFunction.ChiInv(0.05, degree);

            // Считаем точки для нормальной кривой, а также хи квадрат наблюдаемое
            for (int i = 0; i < INTERVAL_AMOUNT; i++)
            {
                // Значение функции плотности нормального распределения
                double t = excel.WorksheetFunction.NormDist((middles[i] - sampleMean) / standardDeviation, 0, 1, false);

                // Теоретические частоты
                n[i] = intervalLen * freqSum / standardDeviation * t;
                chiSquareSeen += (((frequencies[i] - n[i]) * (frequencies[i] - n[i])) / n[i]);
            }
            
            // вывод данных на экран в поля textBox(2-5)
            textBox2.Text = sampleMean.ToString();
            textBox3.Text = standardDeviation.ToString();
            textBox4.Text = chiSquareSeen.ToString();
            textBox5.Text = chiSquareCrit.ToString();
            
            // Проверка критерия Пирсона
            if (chiSquareSeen > chiSquareCrit) 
                label7.Text = "Согласно критерию Пирсона генеральная совокупность не распределена нормально";
            else 
                label7.Text = "Согласно критерию Пирсона генеральная совокупность распределена нормально";

            excel.Quit();
            buildHist(frequencies, n);
            
        }
        double getDeviation(double[] mid, double[] freq, double sampleMean) // ср. квадр. откл
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            double deviation = 0;
            for (int i = 0; i < mid.Length; i++)
                deviation += mid[i] * mid[i] * freq[i];
            deviation = Math.Sqrt((deviation / excel.WorksheetFunction.Sum(freq)) - sampleMean * sampleMean);

            excel.Quit();
            return deviation;
        }
        double getSampleMean(double[] mid, double[] freq) // выбор. среднее
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            double sampleMean = 0;
            for (int i = 0; i < mid.Length; i++)
                sampleMean += mid[i]*freq[i];
            sampleMean /= excel.WorksheetFunction.Sum(freq);

            return sampleMean;
        }

        // Построение гистограммы и нормальной кривой
        void buildHist(double[] histFreq, double[] normCurve)
        {
            // Очищаем гистограмму
            chart1.Series[0].Points.Clear(); 
            chart1.Series[1].Points.Clear();
            // Построение гистограммы
            for (int i = 0; i < INTERVAL_AMOUNT; i++)
            {
               chart1.Series[0].Points.AddXY(i, histFreq[i]);
               chart1.Series[1].Points.AddXY(i, normCurve[i]);
            }
        }
    }
}
