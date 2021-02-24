using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperCalc.Services
{
    public class CalculatorSimulator : ICalcEngine
    {

        Func<double, double, double> add;
        Func<double, double, double> subtract;
        Func<double, double, double> multiply;
        Func<double, double, double> divide;

        private Func<double> equation;

        public double Results
        {
            get
            {
                return equation?.Invoke() ?? 0;
            }
        }

        public CalculatorSimulator()
        {
            add = DoAdd;
            subtract = DoSubtract;
            multiply = DoMultiply;
            divide = DoDivide;
        }

        public bool CanPop
        {
            get
            {
                return equation != null && equation.GetInvocationList().Length > 0;
            }
        }

        public void Clear()
        {
            equation = null;
        }

        public void PopLastCalc()
        {
            var last = equation?.GetInvocationList().LastOrDefault();
            if (last != null)
            {
                var del = (Func<double>)last;
                equation -= del;
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0");
            if (equation != null) //no operations yet
            {
                foreach (var @delegate in equation.GetInvocationList())
                {
                    var func = ((Func<double>)@delegate);
                    string calculationName = func.Method.Name;
                    //yeah I know it is ugly but just getting it done is first
                    if (calculationName.Contains("<ExecuteAdd>"))
                        sb.Append(" + ");
                    else
                    {
                        if (calculationName.Contains("<ExecuteSubtract>"))
                            sb.Append(" - ");
                        else
                        {
                            if (calculationName.Contains("<ExecuteMultiply>"))
                                sb.Append(" X ");
                            else
                            {
                                if (calculationName.Contains("<ExecuteDivide>"))
                                    sb.Append(" / ");
                            }
                        }
                    }
                    double v = func.Invoke();
                    sb.Append(string.Format("{0:N}", v));
                }
            }
            return sb.ToString();
        }

        private double DoAdd(double previousresult, double number)
        {
            if (equation.GetInvocationList().Length > 0)
                return previousresult + number;
            return number;
        }

        private double DoSubtract(double previousresult, double number)
        {
            if (equation.GetInvocationList().Length > 0)
                return previousresult - number;
            return number;
        }

        private double DoMultiply(double previousresult, double number)
        {
            if (equation.GetInvocationList().Length > 0)
                return previousresult * number;
            return number;
        }

        private double DoDivide(double previousresult, double number)
        {
            if (equation.GetInvocationList().Length > 0)
            {
                if (number != 0)
                    return previousresult / number;
                return 0;
            }
            return number;

        }



        public double CurrentValue { get; set; } = 0;


        private void ExecuteAdd(double previousresult, double number)
        {
            equation += () => add(previousresult, number);
        }

        private void ExecuteSubtract(double previousresult, double number)
        {
            equation += () => subtract(previousresult, number);
        }

        private void ExecuteMultiply(double previousresult, double number)
        {
            equation += () => multiply(previousresult, number);
        }

        private void ExecuteDivide(double previousresult, double number)
        {
            equation += () => divide(previousresult, number);
        }

        public void Add(double number)
        {
            ExecuteAdd(Results, number);
        }

        public void Divide(double number)
        {
            ExecuteDivide(Results, number);
        }

        public void Multiply(double number)
        {
            ExecuteMultiply(Results, number);
        }

        public void Subtract(double number)
        {
            ExecuteSubtract(Results, number);
        }

        public void InitializeStartValue(double number)
        {
            equation += () => { return number; };
        }

        public void AddTransaction(MathOperation operation, double value)
        {
            switch (operation)
            {
                case  MathOperation.Init:
                    this.InitializeStartValue(value);
                    break;
                case MathOperation.Add:
                    this.Add(value);
                    break;
                case MathOperation.Subtract:
                    this.Subtract(value);
                    break;
                case MathOperation.Divide:
                    this.Divide(value);
                    break;
                case MathOperation.Multiply:
                    this.Multiply(value);
                    break;
            }
        }
    }

    public enum MathOperation
    {
        Init,
        Add,
        Subtract,
        Divide,
        Multiply,
        Equals
    }
}
