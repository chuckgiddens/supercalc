using SuperCalc.Services;
using SuperCalc.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SuperCalc.ViewModels
{
    public class TenKeyLayoutViewModel : BaseViewModel
    {
        private double _value = 0;
        private double _results = 0;
        private string _valueText = string.Empty;

        private ICalcEngine calc;
        public Command<string> NumberPressCommand { get; }
        public Command<string> CalculationPressCommand { get; }

        public TenKeyLayoutViewModel()
        {
            NumberPressCommand = new Command<string>(UserInputDigit);
            CalculationPressCommand = new Command<string>(ExecuteCalculation);
            calc = DependencyService.Resolve<ICalcEngine>();
        }

        public double DisplayUserInputText
        {
            get
            {
                return _value;
            }
            set
            {
                SetProperty(ref _value, value);
            }
        }

        public double CurrentCalculationResults
        {
            get
            {
                return _results;
            }
            set
            {
                SetProperty(ref _results, value);
                ResetInput();
            }
        }

        private void ExecuteCalculation(string operation)
        {
            switch(operation)
            {
                case "+":
                    CurrentCalculationResults = calc.Add(DisplayUserInputText);
                    break;
            }
            CurrentCalculationResults = calc.Add(_value);
        }

        private void ResetInput()
        {
            _valueText = string.Empty;
            DisplayUserInputText = 0;

        }

        private void UserInputDigit(string value)
        {
            _valueText = _valueText + value;
            if (Double.TryParse(_valueText, out double keyboardnumber))
            {
                DisplayUserInputText = keyboardnumber;
            }
        }
    }
}
