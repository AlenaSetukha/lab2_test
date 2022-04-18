using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClassLibrary1;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewData v_data { get; set; }
        public bool md_filled { get; set; }

        private void Input_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                v_data.Input_Error = false;
                return;
            }
            v_data.Input_Error = true;
            MessageBox.Show($"Input Error: {e.Error.ErrorContent}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void MeasuredData_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !v_data.Input_Error;
        }

        private void Splines_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (!v_data.Input_Error) && md_filled;
        }

        private void MeasuredData_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                First_derivative.Text = "";
                Second_derivative.Text = "";
                Integral1.Text = "";
                Integral2.Text = "";

                v_data.s_data.md.MeasureData_fill();
                md_filled = true;

                // Add to plot
                v_data.char_data.ClearCollection();
                v_data.char_data.AddSeries(v_data.s_data.md.nodes, v_data.s_data.md.res, "Measured", 0);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Splines_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                // Interpolation
                int ret = v_data.s_data.build_spline();

                if (ret == 0)
                {
                    First_derivative.Text = $"First derivative at the ends: " +
                        $"{v_data.s_data.derivatives[0]:0.0000}; {v_data.s_data.derivatives[1]:0.0000}";
                    Second_derivative.Text = $"Second derivative at the ends: " +
                        $"{v_data.s_data.derivatives[2]:0.0000}; {v_data.s_data.derivatives[3]:0.0000}";


                    //Integrals
                    Integral1.Text = $"First Integral: {v_data.s_data.integral_res[0]:0.0000}";
                    Integral2.Text = $"Second Integral: {v_data.s_data.integral_res[1]:0.0000}";
                    // Add to plot
                    double[] points = new double[v_data.s_data.sp.n];
                    for (int i = 0; i < v_data.s_data.sp.n; i++)
                    {
                        points[i] = v_data.s_data.sp.a + i * (v_data.s_data.sp.b - v_data.s_data.sp.a) 
                            / (v_data.s_data.sp.n - 1);
                    }
                    v_data.char_data.AddSeries(points, v_data.s_data.cubic_res, 
                            "Cubic spline", 1);
                }
                else
                {
                    MessageBox.Show($"Error in Spline: {ret}.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public MainWindow()
        {
            v_data = new();
            DataContext = this;
            InitializeComponent();
            Function.ItemsSource = Enum.GetValues(typeof(ClassLibrary1.SPf));
        }

    }

    // Custom Commands
    public static class CustomCommands
    {
        public static readonly RoutedUICommand MeasuredData = new
            (
                "MeasuredData",
                "MeasuredData",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D1, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand Splines = new
            (
                "Splines",
                "Splines",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D2, ModifierKeys.Control)
                }
            );
    }
}
