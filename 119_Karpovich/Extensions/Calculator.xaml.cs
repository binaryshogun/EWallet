using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace _119_Karpovich.Extensions
{
    /// <summary>
    /// Калькулятор.
    /// </summary>
    public partial class Calculator : Window
    {
        #region Fields
        private double temp;
        private double result;
        private bool isNewOperation = false;
        private delegate double Operation(double x, double y);
        private delegate double SingleOperation(double x);
        private Operation operation;
        private SingleOperation singleOperation;
        private Button operationButton;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует окно Calculator.
        /// </summary>
        /// <param name="startupPoint">
        /// Начальная точка появления 
        /// окна относительно экрана.
        /// </param>
        public Calculator(Point startupPoint)
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.Manual;
            Top = startupPoint.Y - 350;
            Left = startupPoint.X;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Метод, очищающий все поля калькулятора.
        /// </summary>
        private void ClearAll()
        {
            temp = 0;
            resultBlock.Content = "0";
            operation = null;
            singleOperation = null;
            isNewOperation = false;
        }
        /// <summary>
        /// Метод, складывающий два действительных числа.
        /// </summary>
        /// <param name="x">Первое слагаемое.</param>
        /// <param name="y">Второе слагаемое.</param>
        /// <returns>Результат сложения двух действительных чисел.</returns>
        private static double Add(double x, double y) => x + y;
        /// <summary>
        /// Метод, вычитающий два действительных числа.
        /// </summary>
        /// <param name="x">Уменьшаемое - действительное число.</param>
        /// <param name="y">Вычитаемое - действительное число.</param>
        /// <returns>Разность двух действительных чисел.</returns>
        private static double Subtract(double x, double y) => x - y;
        /// <summary>
        /// Метод, находящий частное двух чисел.
        /// </summary>
        /// <param name="x">Делимое - действительное число.</param>
        /// <param name="y">Частное - действительное число.</param>
        /// <returns>Частное двух действительных чисел.</returns>
        private static double Divide(double x, double y) => x / y;
        /// <summary>
        /// Метод, находящий произведение двух чисел.
        /// </summary>
        /// <param name="x">Первый множитель - действительное число.</param>
        /// <param name="y">Второй множитель - действительное число.</param>
        /// <returns>Произведение двух действительных чисел.</returns>
        private static double Multiply(double x, double y) => x * y;

        /// <summary>
        /// Метод, возводящий число в квадрат.
        /// </summary>
        /// <param name="x">Число, возводимое в квадрат.</param>
        /// <returns>Число в степени 2 (в квадрате).</returns>
        private static double Sqr(double x) => x * x;
        /// <inheritdoc cref="Math.Sqrt(double)"/>
        private static double Sqrt(double x) => Math.Sqrt(x);
        /// <summary>
        /// Метод, возводящий число в куб.
        /// </summary>
        /// <param name="x">Число, возводимое в 3 степень.</param>
        /// <returns>Число, возведённое в куб (3 степень).</returns>
        private static double Pow(double x) => x * x * x;
        /// <summary>
        /// Метод, меняющий знак числа на противоположный.
        /// </summary>
        /// <param name="x">
        /// Число, для которого необходимо 
        /// изменить знак на противоположный
        /// </param>
        /// <returns>Число с противоположным знаком.</returns>
        private static double ChangeSign(double x) => -1 * x;
        #endregion

        #region EventHandlers
        /// <inheritdoc cref="Window.Deactivated"/>
        private void Calculator_Deactivated(object sender, EventArgs e)
            => Close();

        /// <inheritdoc cref="MouseButtonEventHandler"/>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            => DragMove();

        /// <inheritdoc cref="RoutedEventHandler"/>
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (resultBlock.Content.ToString() == "0" || isNewOperation == true)
                resultBlock.Content = button.Content.ToString().ToString();
            else if (resultBlock.Content.ToString().Length == 6) {} 
            else if (resultBlock.Content.ToString().Contains(",") && button.Content.ToString().ToString() == ",") {}
            else
                resultBlock.Content += button.Content.ToString().ToString();

            result = double.Parse(resultBlock.Content.ToString());
            isNewOperation = false;
        }

        /// <inheritdoc cref="RoutedEventHandler"/>
        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tempOp = operation;

            if (tempOp != null)
                operationButton.Background = button.Background;

            if (operation == null && resultBlock.Content.ToString() != "0")
                temp = Convert.ToDouble(resultBlock.Content.ToString());

            operationButton = button;
            isNewOperation = true;
            double.TryParse(resultBlock.Content.ToString(), out result);

            switch (button.Content.ToString().ToString())
            {
                case "CE":
                    if (operationButton != null)
                        operationButton.Background = button.Background;
                    ClearAll();
                    break;

                case "+":
                    button.Background = Brushes.White;
                    operation = Add;
                    break;

                case "-":
                    button.Background = Brushes.White;
                    operation = Subtract;
                    break;

                case "√":
                    button.Background = Brushes.White;
                    singleOperation = Sqrt;
                    resultBlock.Content = Math.Round(singleOperation(result), 6).ToString();
                    break;

                case "x²":
                    singleOperation = Sqr;
                    resultBlock.Content = Math.Round(singleOperation(result), 6).ToString();
                    break;

                case "x³":
                    button.Background = Brushes.White;
                    singleOperation = Pow;
                    resultBlock.Content = Math.Round(singleOperation(result), 6).ToString();
                    break;

                case "±":
                    button.Background = Brushes.White;
                    singleOperation = ChangeSign;
                    resultBlock.Content = Math.Round(singleOperation(result), 6).ToString();
                    break;

                case "×":
                    button.Background = Brushes.White;
                    operation = Multiply;
                    break;

                case "÷":
                    button.Background = Brushes.White;
                    operation = Divide;
                    break;

                default:
                    break;
            }

            if (tempOp != null && operation != null && singleOperation == null)
                temp = Math.Round(tempOp(temp, Convert.ToDouble(result)), 6);

            if (singleOperation != null)
                result = double.Parse(resultBlock.Content.ToString());

            singleOperation = null;
        }

        /// <inheritdoc cref="RoutedEventHandler"/>
        private void Result_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            operationButton.Background = button.Background;

            if (isNewOperation == false)
                double.TryParse(resultBlock.Content.ToString(), out result);

            if (operation != null)
            {
                resultBlock.Content = Math.Round(operation(temp, result), 6).ToString();
                temp = Convert.ToDouble(resultBlock.Content.ToString());
            }

            isNewOperation = true;
        }
        #endregion
    }
}
