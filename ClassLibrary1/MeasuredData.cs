using System;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ClassLibrary1
{
    public class MeasuredData: IDataErrorInfo
    {
        //неравномерная сетка, проверка на ошибку ввода

        public int n { get; set; }
        public double a { get; set; }
        public double b { get; set; }
        public SPf f { get; set; }

        public double[] nodes { get; set; }
        public double[] res { get; set; }
        public ObservableCollection<string> col { get; set; }


        public MeasuredData(int nn, double start, double end, SPf fin)
        {
            n = nn;
            a = start;
            b = end;
            f = fin;
            col = new();
        }

        public void MeasureData_fill()
        {
            nodes = new double[n];
            Random rnd = new Random();
            nodes[0] = a;
            nodes[n - 1] = b;
            for (int i = 1; i < n - 1; i++)
            {
                nodes[i] = a + rnd.NextDouble() * (b - a);
            }
            Array.Sort(nodes);

            res = new double[n];
            if (f == SPf.linear)
            {
                for (int i = 0; i < n; i++)
                {
                    res[i] = 2 * nodes[i] + 1.0;//2x + 1
                }
            }
            if (f == SPf.cubic)
            {
                for (int i = 0; i < n; i++)
                {
                    res[i] = 3 * nodes[i] * nodes[i] * nodes[i] + 2.0;//3x^3 + 2
                }
            }

            if (f == SPf.rand)
            {
                for (int i = 0; i < n; i++)
                {
                    res[i] = 5.0 * rnd.NextDouble();//rand
                }
            }

            col.Clear();
            for (int i = 0; i < n; i++)
            {
                col.Add($"Nod: {nodes[i]:0.00000}  F(nod): {res[i]:0.00000}\n");
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string field_name]
        {
            get
            {
                string error = null;
                switch (field_name)
                {
                    case "a":
                    case "b":
                        if (b <= a)
                        {
                            error = "b < a";
                        }
                        break;
                    case "n":
                        if (n < 3)
                        {
                            error = "Длина меньше 3";
                        }
                        break;
                    default:
                        break;
                }
                return error;
            }
        }
    }
}
