using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace WpfApp1
{
    class LimitsConverter: IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)//из x1,x2,x3 в строку
        {
            try
            {
                string res = value[0].ToString() + " " +
                    value[1].ToString() + " " + value[2].ToString();
                return res;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка конвертации LimitsConverter.Convert");
                object res = "0 " + "0 " + "0";
                return res;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)//из строки в x1, x2, x3
        {
            try
            {
                string s = value as string;
                string[] words = s.Split(' ');
                object[] res = new object[3];
                res[0] = double.Parse(words[0]);//x1
                res[1] = double.Parse(words[1]);//x2
                res[2] = double.Parse(words[2]);//x3
                return res;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка LimitsConverter.Convertback");
                object[] res = new object[3];
                res[0] = 0.0;
                res[1] = 1.0;
                res[2] = 2.0;
                return res;
            }
        }
    }
}