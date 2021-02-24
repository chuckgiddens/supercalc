using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperCalc.Services
{
    public interface ICalcEngine
    {
        void Add(double number);
        void Subtract(double number);
        void Divide(double number);
        void Multiply(double number);
        void InitializeStartValue(double number);
        double Results { get; }
        void Clear();
        void PopLastCalc();
        bool CanPop { get; }
        void AddTransaction(MathOperation operation, double value);
    }
}
