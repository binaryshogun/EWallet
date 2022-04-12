using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _119_Karpovich.Extensions
{
    /// <summary>
    /// Логика взаимодействия для Calculator.xaml
    /// </summary>
    public partial class Calculator : Window
    {
        public Calculator(Point startupPoint)
        {
            InitializeComponent();

            var uri = new Uri("Resources/CalculatorTheme.xaml", UriKind.Relative);
            ResourceDictionary resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
            Resources.MergedDictionaries.Add(resourceDictionary);

            WindowStartupLocation = WindowStartupLocation.Manual;
            Top = startupPoint.Y - 350;
            Left = startupPoint.X;
        }

        private void Calculator_Deactivated(object sender, EventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (resultBlock.Text == "0") 
                resultBlock.Text = button.Content.ToString();
            else if (resultBlock.Text.Length == 6) { }
            else if (resultBlock.Text.Contains(",") && button.Content.ToString() == ",") { }
            else 
                resultBlock.Text += button.Content.ToString(); 

            Result = resultBlock.Text;
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (resultBlock.Text != "0")
            {
                Temp = Convert.ToDouble(Result);
                resultBlock.Text = "0";
            }

            switch (button.Content.ToString())
            {
                case "CE":
                    Result = ClearAll();
                    Temp = 0;
                    resultBlock.Text = Result;
                    operation = null;
                    singleOperation = null;
                    break;

                case "+":
                    operation = Add;
                    break;

                case "-":
                    operation = Subtract;
                    break;

                case "√":
                    singleOperation = Sqrt;
                    resultBlock.Text = Math.Round(singleOperation(Temp), 3).ToString();
                    Result = resultBlock.Text;
                    break;

                case "x²":
                    singleOperation = Sqr;
                    resultBlock.Text = Math.Round(singleOperation(Temp), 3).ToString();
                    Result = resultBlock.Text;
                    break;

                case "x³":
                    singleOperation = Pow;
                    resultBlock.Text = Math.Round(singleOperation(Temp), 3).ToString();
                    Result = resultBlock.Text;
                    break;

                case "±":
                    singleOperation = ChangeSign;
                    resultBlock.Text = Math.Round(singleOperation(Temp), 3).ToString();
                    Result = resultBlock.Text;
                    break;

                case "×":
                    operation = Multiply;
                    break;

                case "÷":
                    operation = Divide;
                    break;

                default:
                    break;
            }
        }

        private void Result_Click(object sender, RoutedEventArgs e)
        {
            if (operation != null)
            {
                resultBlock.Text = Math.Round(operation(Temp, Convert.ToDouble(Result)), 3).ToString();
                Temp = Convert.ToDouble(resultBlock.Text);
            }
        }

        private static string ClearAll() => "0";
        private static double Add(double x, double y) => x + y;
        private static double Subtract(double x, double y) => x - y;
        private static double Divide(double x, double y) => x / y;
        private static double Multiply(double x, double y) => x * y;

        private static double Sqr(double x) => x * x;
        private static double Sqrt(double x) => Math.Sqrt(x);
        private static double Pow(double x) => x * x * x;
        private static double ChangeSign(double x) => -1 * x;

        public string Result { get; set; }
        public double Temp { get; set; }

        delegate double Operation(double x, double y);
        delegate double SingleOperation(double x);
        Operation operation;
        SingleOperation singleOperation;
    }
}
