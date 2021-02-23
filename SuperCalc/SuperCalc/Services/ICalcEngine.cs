using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperCalc.Services
{
    public interface ICalcEngine
    {
        double Add(double number);
        double Subtract(double number);
        double Divide(double number);
        double Multiply(double number);
    }
}
