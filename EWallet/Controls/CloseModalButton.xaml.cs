using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace EWallet.Controls
{
    /// <summary>
    /// Логика взаимодействия для CloseModalButton.xaml
    /// </summary>
    public partial class CloseModalButton : UserControl
    {
        public CloseModalButton()
        {
            InitializeComponent();

            Binding bindCommand = new Binding();
            bindCommand.Source = this;
            bindCommand.Path = new PropertyPath("Command");
            bindCommand.Mode = BindingMode.TwoWay;
            button.SetBinding(Button.CommandProperty, bindCommand);
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CloseModalButton), new PropertyMetadata(null));
    }
}
