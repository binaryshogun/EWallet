using _119_Karpovich.Extensions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace _119_Karpovich.Commands
{
    public class OpenCalculatorCommand<TWindow> : CommandBase
        where TWindow : Calculator
    {
        public OpenCalculatorCommand() { }

        public async override void Execute(object parameter)
        {
            calculator = new Calculator(FindPoint.GetCursorPosition());
            await Task.Run(
                () => Application.Current.Dispatcher.Invoke(
                    () => calculator.Show()));
        }

        private Calculator calculator;
    }

    class FindPoint
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
    }
}
