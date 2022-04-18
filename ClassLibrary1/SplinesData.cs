using System.Runtime.InteropServices;
using System.ComponentModel;
using System;

namespace ClassLibrary1
{
    public class SplinesData: IDataErrorInfo
    {
        public MeasuredData md { get; set; }    //неравномерная сетка
        public SplineParameters sp { get; set; }//равномерная сетка

        public double[] cubic_res { get; set; }//кубический сплайн на равномернеой сетке
        public double[] integral_res { get; set; } = new double[2];
        public double[] derivatives { get; set; } = new double[4];//значения производных на концах

        public SplinesData(MeasuredData md_in, SplineParameters sp_in)
        {
            md = md_in;
            sp = sp_in;
        }

        public SplinesData(SplinesData obj)
        {
            md = obj.md;
            sp = obj.sp;
        }

        [DllImport("..\\..\\..\\..\\x64\\Debug\\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Global_Spline(double[] v, int n, double[] vals, int n_uniform,
                double[] ab_uniform, double[] res);
        [DllImport("..\\..\\..\\..\\x64\\Debug\\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Global_Integral(double[] v, int n, double[] vals, int n_uniform,
                double[] segment, double[] res_integral);

        public int build_spline()
        {
            int ret;
            double[] spline_res = new double[3 * sp.n];
            ret = Global_Spline(md.nodes, md.n, md.res, sp.n,
                    new double[] { sp.a, sp.b }, spline_res);
            if (ret != 0)
            {
                return ret;
            }

            double[] resault = new double[sp.n];
            for (int i = 0; i < sp.n; i++)
            {
                resault[i] = spline_res[3 * i];//вернул тройками(f, f', f")
            }
            cubic_res = resault;


            derivatives[0] = spline_res[1];
            derivatives[1] = spline_res[(3 * sp.n) - 2];
            derivatives[2] = spline_res[2];
            derivatives[3] = spline_res[(3 * sp.n) - 1];




            ret = Global_Integral(md.nodes, md.n, md.res, sp.n,
                    new double[] { sp.x1, sp.x2, sp.x3 }, integral_res);
            

            if (ret != 0)
            {
                return ret;
            }    

            return 0;
        }


        public string this[string field_name]
        {
            get
            {
                string error = null;
                if (field_name.Equals("sp.a"))
                {
                    if (sp.a != md.a)
                    {
                        error = "Неравные отрезки для Measure и Spline";
                    }

                }
                if (field_name.Equals("sp.b"))
                {
                    if (sp.b != md.b)
                    {
                        error = "Неравные отрезки для Measure и Spline";
                    }
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
