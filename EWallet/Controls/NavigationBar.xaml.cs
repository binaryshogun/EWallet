using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EWallet.Components
{
    /// <summary>
    /// Логика взаимодействия для NavigationBar.xaml
    /// </summary>
    public sealed partial class NavigationBar : UserControl
    {
        #region Constructors
        public NavigationBar() => InitializeComponent();
        #endregion

        #region Methods
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
            => Application.Current.MainWindow.DragMove();
        #endregion
    }
}
