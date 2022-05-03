using _119_Karpovich.Extensions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace _119_Karpovich.Commands
{
    /// <summary>
    /// Команда открытия окна калькулятора.
    /// </summary>
    /// <typeparam name="TWindow">Класс калькулятора.</typeparam>
    internal class OpenCalculatorCommand : CommandBase
    {
        #region Fields
        private Calculator calculator;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует команду открытия окна колькулятора.
        /// </summary>
        public OpenCalculatorCommand() { }
        #endregion

        #region Methods
        ///<inheritdoc cref="CommandBase.Execute(object)"/>
        public async override void Execute(object parameter)
        {
            calculator = new Calculator(FindPoint.GetCursorPosition());
            await Task.Run(
                () => Application.Current.Dispatcher.Invoke(
                    () => calculator.Show()));
        }
        #endregion
    }

    /// <summary>
    /// Класс для поиска точки,
    /// в которой находится курсор мыши
    /// относительно экрана.
    /// </summary>
    class FindPoint
    {
        #region Fields
        /// <summary>
        /// Структура точка.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PointStruct
        {
            public int X;
            public int Y;

            public static implicit operator Point(PointStruct point) 
                => new Point(point.X, point.Y);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Метод получения точки, в которой находится курсор.
        /// </summary>
        /// <returns>Точка экрана, в которой находится курсор.</returns>
        public static Point GetCursorPosition()
        {
            bool success = GetCursorPos(out PointStruct lpPoint);
            return !success ? new Point(0, 0) : lpPoint;
        }
        #endregion

        #region ImportedDLLs
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out PointStruct lpPoint);
        #endregion
    }
}
