using System;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace WpfApp1
{
    public class ChartData
    {
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }

        public ChartData(double[] inLables)
        {
            SeriesCollection = new SeriesCollection();

            // Values Formatter
            Formatter = value => value.ToString("F4");
        }

        // Add plot series
        public void AddSeries(double[] points, double[] values, string title, int mode)
        {
            ChartValues<ObservablePoint> Values = new ChartValues<ObservablePoint>();
            for (int i = 0; i < points.Length; i++)
            {
                Values.Add(new(points[i], values[i]));
            }

            if (mode == 0)//measured
            {
                SeriesCollection.Add(new ScatterSeries
                {
                    Title = title,
                    Values = Values,
                    Fill = Brushes.Red,
                    MinPointShapeDiameter = 5,
                    MaxPointShapeDiameter = 5
                });
            }
            else if (mode == 1)//spline
            {
                SeriesCollection.Add(new LineSeries
                {
                    Title = title,
                    Values = Values,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Green,
                    PointGeometry = null // Line without markers
                });
            }
        }

        // Clear plot
        public void ClearCollection()
        {
            SeriesCollection.Clear();
        }
    }
}
