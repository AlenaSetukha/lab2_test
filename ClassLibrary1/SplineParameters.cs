using System;
using System.ComponentModel;

namespace ClassLibrary1
{
    public class SplineParameters: IDataErrorInfo
    {
        //равномерная сетка, проверка на ошибку ввода
        public int n { get; set; }       
        public double a { get; set; }
        public double b { get; set; }
        public double x1 { get; set; }
        public double x2 { get; set; }
        public double x3 { get; set; }


        public SplineParameters(int nn, double start, double end, double x11, double x22, double x33)
        {
            n = nn;
            a = start;
            b = end;
            x1 = x11;
            x2 = x22;
            x3 = x33;
        }

        public string this[string field_name]
        {
            get
            {
                string error = null;
                switch (field_name)
                {
                    case "n":
                        if (n < 3)
                        {
                            error = "Длина меньше 3";
                        }
                        break;
                    case "a":
                    case "b":
                        if (b <= a)
                        {
                            error = "b < a";
                        }
                        break;
                    case "x1":
                    case "x2":
                    case "x3":
                        if ((x1 >= x2) || (x2 >= x3) || (x1 < a) || (x3 > b))
                        {
                            error = "Нарушение a <= x1 < x2 < x3 <= b";
                        }
                        break;
                    default:
                        break;
                }
                return error;
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
