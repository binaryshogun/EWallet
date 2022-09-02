using EWallet.Commands;
using EWallet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

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
