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
using System.Windows.Forms.DataVisualization.Charting;

namespace Test
{
    public partial class WeibullDistributionView : Form
    {
        private WeibullDistributionViewModel _viewModel;
        public WeibullDistributionView()
        {
            _viewModel = new WeibullDistributionViewModel();
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                double k = Convert.ToDouble(kTextBox.Text);
                double lambda = Convert.ToDouble(lambdaTextBox.Text);
                int numberAmount = Convert.ToInt32(numberAmountTextBox.Text);
                int intervalAmount = Convert.ToInt32(intervalAmountTextBox.Text);

                _viewModel.Calculate(k, lambda, numberAmount, intervalAmount);
                chiSquareObservableTextBox.Text = Convert.ToString(_viewModel.chiExperimental);
                chiSquareCriticalTextBox.Text = Convert.ToString(_viewModel.chiCritical);
                BuildHistogram();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occured!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuildHistogram()
        {
           int index = 0;
           
           chart1.Series[0].Points.Clear();
           chart1.Series[1].Points.Clear();

            for (int i = 0; i < _viewModel.intervalAmount; i++)
            {
                chart1.Series[0].Points.AddXY(_viewModel.Middles[i], _viewModel.Frequencies[i]);
                chart1.Series[1].Points.AddXY(_viewModel.Middles[i], _viewModel.CurvePoints[i]);
            }

           /*for (double i = 0; i < _viewModel.generator.Intervals[_viewModel.intervalAmount - 1].RightBorder; i += 0.1)
           {
                chart1.Series[1].Points.AddXY(i, _viewModel.CurvePoints[index]);
                index++;
           }*/
        }

        private void WeibullDistributionView_KeyPress(object sender, KeyPressEventArgs e)
        {
            button1_Click_1(new object(), new EventArgs());
        }
    }
}
