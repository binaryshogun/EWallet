using EWallet.Commands;
using EWallet.Services;
using System.Windows.Input;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel домашней страницы.
    /// </summary>
    public sealed class HomeViewModel : ViewModelBase
    {
        #region Constructors
        public HomeViewModel(INavigationService authorizationNavigationService,
            INavigationService registrationNavigationService)
        {
            NavigateAuthorizationCommand = new NavigateCommand(
                authorizationNavigationService);
            NavigateRegistrationCommand = new NavigateCommand(
                registrationNavigationService);
        }
        #endregion

        #region Commands
        public ICommand NavigateRegistrationCommand { get; }
        public ICommand NavigateAuthorizationCommand { get; }
        #endregion

        #region Methods
        public override void Dispose() => base.Dispose();
        #endregion
    }
}
