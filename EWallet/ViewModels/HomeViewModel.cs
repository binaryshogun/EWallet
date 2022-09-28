using EWallet.Commands;
using EWallet.Services;
using System.Windows.Input;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel домашней страницы.
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(INavigationService authorizationNavigationService,
            INavigationService registrationNavigationService)
        {
            NavigateAuthorizationCommand = new NavigateCommand(
                authorizationNavigationService);
            NavigateRegistrationCommand = new NavigateCommand(
                registrationNavigationService);
        }

        public ICommand NavigateRegistrationCommand { get; }
        public ICommand NavigateAuthorizationCommand { get; }

        public override void Dispose() => base.Dispose();
    }
}
