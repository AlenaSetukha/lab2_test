using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace WpfApp1
{
    class SegmentConverter: IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)//из MeasureData в строку
        {
            try
            {
                string res = value[0].ToString() + " " +
                    value[1].ToString() + " " + value[2].ToString();
                return res;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка конвертации SegmentConverter.Convert");
                object res = "0 " + "0 " + "0";
                return res;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)//из строки в Meausure
        {
            try
            {
                string s = value as string;
                string[] words = s.Split(' ');
                object[] res = new object[3];
                res[0] = double.Parse(words[0]);//a
                res[1] = double.Parse(words[1]);//b
                res[2] = Int32.Parse(words[2]);//n
                return res;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка SegmentConverter.Convertback");
                object[] res = new object[3];
                res[0] = 0.0;
                res[1] = 1.0;
                res[2] = 10;
                return res;
            }
        }
    }
}
