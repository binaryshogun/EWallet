using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EWallet.Controls
{
    /// <summary>
    /// Логика взаимодействия для OperationButton.xaml
    /// </summary>
    public sealed partial class OperationButton : UserControl
    {
        public OperationButton()
        {
            InitializeComponent();

            Binding bindOperation = new Binding();
            bindOperation.Source = this;
            bindOperation.Path = new PropertyPath("Operation");
            operation.SetBinding(TextBlock.TextProperty, bindOperation);

            Binding bindOperationDescription = new Binding();
            bindOperationDescription.Source = this;
            bindOperationDescription.Path = new PropertyPath("OperationDescription");
            operationDescription.SetBinding(TextBlock.TextProperty, bindOperationDescription);
        }

        public string Operation
        {
            get => (string)GetValue(OperationProperty);
            set => SetValue(OperationProperty, value);
        }

        // Using a DependencyProperty as the backing store for Operation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OperationProperty =
            DependencyProperty.Register("Operation", typeof(string), typeof(OperationButton), new PropertyMetadata(""));

        public string OperationDescription
        {
            get => (string)GetValue(OperationDescriptionProperty);
            set => SetValue(OperationDescriptionProperty, value);
        }

        // Using a DependencyProperty as the backing store for OperationDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OperationDescriptionProperty =
            DependencyProperty.Register("OperationDescription", typeof(string), typeof(OperationButton), new PropertyMetadata(""));
    }
}
