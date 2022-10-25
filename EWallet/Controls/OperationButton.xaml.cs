using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EWallet.Controls
{
    /// <summary>
    /// Логика взаимодействия для OperationButton.xaml
    /// </summary>
    public sealed partial class OperationButton : Button
    {
        #region Constructors
        public OperationButton()
        {
            InitializeComponent();

            Binding bindOperation = new Binding("Operation")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };
            operation.SetBinding(TextBlock.TextProperty, bindOperation);

            Binding bindOperationDescription = new Binding("OperationDescription")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };
            operationDescription.SetBinding(TextBlock.TextProperty, bindOperationDescription);

            Binding bindImage = new Binding("ImageSource")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };
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

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(OperationButton), new PropertyMetadata("../Resources/images/refill.png"));
        #endregion
    }
}
