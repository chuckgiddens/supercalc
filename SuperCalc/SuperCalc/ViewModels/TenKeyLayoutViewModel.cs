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
        public Command<MathOperation> CalculationPressCommand { get; }
        public Command PopLastCalcCommand { get; }
        public Command RemoveLastDigitCommand { get; }
        public TenKeyLayoutViewModel()
        {
            NumberPressCommand = new Command<string>(UserInputDigit);
            CalculationPressCommand = new Command<MathOperation>(ExecuteCalculation, (s) => { return _valueText.Length > 0; });
            PopLastCalcCommand = new Command(PopLastCalculation, () => { return calc.CanPop; });
            RemoveLastDigitCommand = new Command(RemoveLastDigit, () => { return _valueText.Length > 0; });
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

        private void RemoveLastDigit()
        {
            if (_valueText.Length > 0)
                _valueText = _valueText.Substring(0, _valueText.Length - 1);
            UpdateDisplay();
        }

        private void PopLastCalculation()
        {
            calc.PopLastCalc();
            CurrentCalculationResults = calc.Results;
            if (!calc.CanPop)
                NextOperation = MathOperation.Init; //start over
            PopLastCalcCommand.ChangeCanExecute();
        }

        MathOperation _nextOperation = MathOperation.Init;
        public MathOperation NextOperation 
        {
            get => _nextOperation;
            set => SetProperty(ref _nextOperation, value);
        }

        private void ExecuteCalculation(MathOperation operation)
        {
            if (!string.IsNullOrEmpty(_valueText))
            {
                calc.AddTransaction(NextOperation, DisplayUserInputText);
                CurrentCalculationResults = calc.Results;
                NextOperation = operation;
            }
            if (NextOperation == MathOperation.Equals) //starting over so init the saved value to 0
            {
                calc.Clear();
                NextOperation = MathOperation.Init;
            }
            PopLastCalcCommand.ChangeCanExecute();
            CalculationPressCommand.ChangeCanExecute();
        }

        private void ResetInput()
        {
            _valueText = string.Empty;
            DisplayUserInputText = 0;
            RemoveLastDigitCommand.ChangeCanExecute();
        }

        private void UserInputDigit(string value)
        {
            _valueText = _valueText + value;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (_valueText.Length > 0)
            {
                if (Double.TryParse(_valueText, out double keyboardnumber))
                {
                    DisplayUserInputText = keyboardnumber;
                }
            }
            else
                DisplayUserInputText = 0;
            RemoveLastDigitCommand.ChangeCanExecute();
            CalculationPressCommand.ChangeCanExecute();
        }
    }
}
