using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    using OxyPlot;
    using OxyPlot.Series;

    public partial class MainWindow : Window
    {
        double R = 0, L = 0, C = 0;
        public PlotModel MyModel { get; private set; }

        public LineSeries lineSeries1;

        private OxyPlot.Axes.LinearAxis _axisY, _axisX;

        public double UcU(int w_t)
        {
            double Xc = 1 / (w_t * C);
            double Xl = w_t * L;
            double Im = 1 / Math.Sqrt(R * R + ((Xl - Xc) * (Xl - Xc)));
            double Uc = Xc * Im;
            return Uc;
        }

        public MainWindow()
        {
            InitializeComponent();
            MyModel = new PlotModel { Title = "Резонансная кривая" };
            graph.Model = MyModel;

            MyModel.Axes.Clear();
            this.lineSeries1 = new LineSeries ();
            _axisX = new OxyPlot.Axes.LinearAxis()
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom
            };
            _axisY = new OxyPlot.Axes.LinearAxis()
            {
                Position = OxyPlot.Axes.AxisPosition.Left
            };
            MyModel.Axes.Add(_axisX);
            MyModel.Axes.Add(_axisY);
            MyModel.InvalidatePlot(true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            _axisX.Title = "ω, Гц";
            _axisY.Title = "Uc/U";

            double Rin, Lin, Cin;
            
            bool correct_input = true;

            List<double> values = new List<double> { };
            lineSeries1 = new LineSeries();

            textBlock_Xc.Text = "Xc = ";
            textBlock_XL.Text = "XL = ";
            textBlock_Q.Text =  "Q = ";
            textBlock_w0.Text = "ω0 = ";

            if (double.TryParse(textBox_R.Text, out Rin))
            {
                R = Rin;
            }
            else
            {
                correct_input = false;
            }

            if (double.TryParse(textBox_L.Text, out Lin))
            {
                L = Lin / 1000;
            }
            else
            {
                correct_input = false;
            }

            if (double.TryParse(textBox_C.Text, out Cin))
            {
                C = Cin / 1000000;
            }
            else
            {
                correct_input = false;
            }

            if ((R < 0) || (C <= 0) || (L <= 0))
            {
                correct_input = false;
            }


            if (correct_input)
            {
                
                double Q = Math.Round((Math.Sqrt(L / C) / R) * 1000) / 1000;

                // w0, при котором достигается резонанс
                double w0 = Math.Round((1 / Math.Sqrt(L * C)) * 1000) / 1000; 

                double Xc = Math.Round((1 / (w0 * C)) * 100) / 100;
                double XL = Math.Round((w0 * L) * 100) / 100;

                textBlock_Xc.Text += (Xc.ToString() + " Ом");
                textBlock_XL.Text += (XL.ToString() + " Ом");
                textBlock_Q.Text  += Q.ToString();
                textBlock_w0.Text += (w0.ToString() + " Гц");

                double max = 0;

                for (int w = 1; w < w0 * 2; w++)
                {
                    values.Add(UcU(w));
                    lineSeries1.Points.Add(new DataPoint(w, values[w - 1]));
                    if (values[w - 1] > max)
                        max = values[w - 1];

                }

                max = Math.Round(max * 1000) / 1000;
                MyModel.Series.Clear();
                MyModel.Series.Add(lineSeries1);

                graph.Model.InvalidatePlot(true);
                graph.InvalidatePlot(true);
                textBlock_out.Text = max.ToString();
            }
            else
            {
                MessageBox.Show("Некорректные данные");
            }
        }
    }
}
