using SuperCalc.Services;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class TestSuperCalcOperations
    {
        [Fact]
        public void TestAllCalculations()
        {
            ICalcEngine calc = new CalculatorSimulator();
            calc.InitializeStartValue(10);
            calc.Add(20);
            Assert.Equal(30, calc.Results);
            calc.Subtract(10);
            Assert.Equal(20, calc.Results);
            calc.Multiply(5);
            Assert.Equal(100,calc.Results);
            calc.PopLastCalc();
            Assert.Equal(20, calc.Results);
            calc.Clear();
            Assert.Equal(0, calc.Results);
        }
    }
}
