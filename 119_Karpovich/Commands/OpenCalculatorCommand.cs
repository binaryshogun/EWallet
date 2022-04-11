using _119_Karpovich.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace _119_Karpovich.Commands
{
    public class OpenCalculatorCommand<TWindow> : CommandBase
        where TWindow : Calculator
    {
        public OpenCalculatorCommand()
        {
            _calculator = new Calculator();
        }
        public async override void Execute(object parameter)
        {
            await Task.Run(
                () => Application.Current.Dispatcher.Invoke(
                    () => _calculator.Show()));
        }

        private readonly Calculator _calculator;
    }
}
