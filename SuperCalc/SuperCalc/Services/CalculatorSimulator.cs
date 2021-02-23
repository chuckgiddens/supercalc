using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCalc.Services
{
    public class CalculatorSimulator : ICalcEngine
    {
        public double CurrentValue { get; set; } = 0;

        public double Add(double number)
        {
            CurrentValue += number;
            return CurrentValue;
        }

        public double Divide(double number)
        {
            //a little defensive programming
            if (number == 0)
                return CurrentValue;
            CurrentValue = CurrentValue / number;
            return CurrentValue;
        }

        public double Multiply(double number)
        {
            CurrentValue = CurrentValue * number;
            return number;
        }

        public double Subtract(double number)
        {
            CurrentValue -= number;
            return CurrentValue;
        }
    }
}
