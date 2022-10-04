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
        public OperationButton()
        {
            InitializeComponent();

            Binding bindOperation = new Binding();
            bindOperation.Source = this;
            bindOperation.Path = new PropertyPath("Operation");
            bindOperation.Mode = BindingMode.TwoWay;
            operation.SetBinding(TextBlock.TextProperty, bindOperation);

            Binding bindOperationDescription = new Binding();
            bindOperationDescription.Source = this;
            bindOperationDescription.Path = new PropertyPath("OperationDescription");
            bindOperationDescription.Mode = BindingMode.TwoWay;
            operationDescription.SetBinding(TextBlock.TextProperty, bindOperationDescription);

            Binding bindCommand = new Binding();
            bindCommand.Source = this;
            bindCommand.Path = new PropertyPath("Command");
            bindCommand.Mode = BindingMode.TwoWay;
            button.SetBinding(Button.CommandProperty, bindCommand);

            Binding bindImage = new Binding();
            bindImage.Source = this;
            bindImage.Path = new PropertyPath("ImageSource");
            bindImage.Mode = BindingMode.OneWay;
            operationImage.SetBinding(Image.SourceProperty, bindImage);
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

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(OperationButton), new PropertyMetadata(null));

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(OperationButton), new PropertyMetadata(""));


    }
}
