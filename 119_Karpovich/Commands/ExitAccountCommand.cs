using _119_Karpovich.Services;
using _119_Karpovich.ViewModels;
using System.Windows;

namespace _119_Karpovich.Commands
{
    internal class ExitAccountCommand<TViewModel> : CommandBase
        where TViewModel : AuthorizationViewModel
    {
        private readonly NavigationService<TViewModel> navigationService;

        public ExitAccountCommand(NavigationService<TViewModel> navigationService)
        {
            this.navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            if (MessageBox.Show("Вы действительно хотите выйти?", "Предупреждение", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                navigationService.Navigate();
            }
        }
    }
}
