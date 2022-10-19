using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace EWallet.Controls
{
    /// <summary>
    /// Логика взаимодействия для OperationButton.xaml
    /// </summary>
    public sealed partial class OperationButton : UserControl
    {
        #region Constructors
        public OperationButton()
        {
            InitializeComponent();

            Binding bindOperation = new Binding("Operation");
            bindOperation.Source = this;
            bindOperation.Mode = BindingMode.OneWay;
            operation.SetBinding(TextBlock.TextProperty, bindOperation);

            Binding bindOperationDescription = new Binding("OperationDescription");
            bindOperationDescription.Source = this;
            bindOperationDescription.Mode = BindingMode.OneWay;
            operationDescription.SetBinding(TextBlock.TextProperty, bindOperationDescription);

            Binding bindCommand = new Binding("Command");
            bindCommand.Source = this;
            bindCommand.Mode = BindingMode.OneWay;
            button.SetBinding(Button.CommandProperty, bindCommand);

            Binding bindImage = new Binding("ImageSource");
            bindImage.Source = this;
            bindImage.Mode = BindingMode.OneWay;
            operationImage.SetBinding(Image.SourceProperty, bindImage);
        }
        #endregion

        #region Properties
        public string Operation
        {
            get => (string)GetValue(OperationProperty);
            set => SetValue(OperationProperty, value);
        }
        public string OperationDescription
        {
            get => (string)GetValue(OperationDescriptionProperty);
            set => SetValue(OperationDescriptionProperty, value);
        }
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
        #endregion

        #region Dependency properties
        public static readonly DependencyProperty OperationProperty =
            DependencyProperty.Register("Operation", typeof(string), typeof(OperationButton), new PropertyMetadata(""));

        public static readonly DependencyProperty OperationDescriptionProperty =
            DependencyProperty.Register("OperationDescription", typeof(string), typeof(OperationButton), new PropertyMetadata(""));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(OperationButton), new PropertyMetadata(null));

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(OperationButton), new PropertyMetadata("../Resources/images/refill.png"));
        #endregion
    }
}
