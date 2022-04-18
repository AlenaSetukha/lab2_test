// pch.cpp: файл исходного кода, соответствующий предварительно скомпилированному заголовочному файлу

#include "pch.h"

// При использовании предварительно скомпилированных заголовочных файлов необходим следующий файл исходного кода для выполнения сборки.
#include "mkl.h"
#include "mkl_df_defines.h"

extern "C" _declspec(dllexport) int Global_Spline(const double* v, const int n, const double* vals,
    const int n_uniform, const double* ab_uniform, double* res)//ab_uniform - массив из 2 элементов
{
    //Create task
    DFTaskPtr task;
    int ret = dfdNewTask1D(&task, n, v, DF_NON_UNIFORM_PARTITION, 1, vals, DF_NO_HINT);

    if (ret != DF_STATUS_OK)
    {
        return 1;
    }

    //Task configuration
    const double* scoeff = new double[(n - 1) * DF_PP_CUBIC];//ny = 1
//    const double* scoeff = new double[(n_uniform - 1) * DF_PP_CUBIC];//ny = 1
    ret = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_FREE_END,
        NULL, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
    if (ret != DF_STATUS_OK)
    {
        return 2;
    }
    //Building a spline
    ret = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
    if (ret != DF_STATUS_OK)
    {
        return 3;
    }

    //Value calculation for UNIFORM grid
    const MKL_INT dorder[] = { 1, 1, 1 };//считаем 0, 1, 2 производные

    ret = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, n_uniform, ab_uniform,
        DF_UNIFORM_PARTITION, 3, dorder, NULL, res, DF_NO_HINT, NULL);

    if (ret != DF_STATUS_OK)
    {
        return 4;
    }
    //Destructor
    ret = dfDeleteTask(&task);
    if (ret != DF_STATUS_OK)
    {
        return 5;
    }
    return 0;
}



extern "C" _declspec(dllexport) int Global_Integral(const double* v, const int n,
    const double* vals, const int n_uniform, const double* limits, double* res_integral)
{
    //Create task
    DFTaskPtr task;
    int ret = dfdNewTask1D(&task, n, v, DF_NON_UNIFORM_PARTITION, 1, vals, DF_NO_HINT);
    //последний парам DF_NO_HINT 
    if (ret != DF_STATUS_OK)
    {
        return -1;
    }

    //Task configuration
    const double* scoeff = new double[(n - 1) * DF_PP_CUBIC];//ny = 1
//    const double* scoeff = new double[(n_uniform - 1) * DF_PP_CUBIC];//ny = 1
    ret = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_FREE_END,
        NULL, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
    if (ret != DF_STATUS_OK)
    {
        return -2;
    }
    //Building a spline
    ret = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
    if (ret != DF_STATUS_OK)
    {
        return -3;
    }



    //Calculation of integrals
    double* llim = new double[2];
    llim[0] = limits[0];
    llim[1] = limits[1];
    double* rlim = new double[2];
    rlim[0] = limits[1];
    rlim[1] = limits[2];

    ret = dfdIntegrate1D(task, DF_METHOD_PP, 2, llim, DF_UNIFORM_PARTITION,
        rlim, DF_UNIFORM_PARTITION, NULL, NULL, res_integral, DF_NO_HINT);//DF_MATRIX_STORAGE_ROWS
    if (ret != DF_STATUS_OK)
    {
        return -4;
    }
    //Destructor
    ret = dfDeleteTask(&task);
    if (ret != DF_STATUS_OK)
    {
        return -5;
    }
    return 0;
}