using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EWallet.Components
{
    /// <summary>
    /// Логика взаимодействия для NavigationBar.xaml
    /// </summary>
    public partial class NavigationBar : UserControl
    {
        public NavigationBar()
        {
            InitializeComponent();
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
            => Application.Current.MainWindow.DragMove();
    }
}
