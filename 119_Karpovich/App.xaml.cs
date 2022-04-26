using _119_Karpovich.Stores;
using _119_Karpovich.ViewModels;
using System.Windows;

namespace _119_Karpovich
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new AuthorizationViewModel(navigationStore);

            MainWindow = new MainWindow()
            {
                DataContext = new DisplayViewModel(navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
