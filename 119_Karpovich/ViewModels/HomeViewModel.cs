using _119_Karpovich.Commands;
using _119_Karpovich.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace _119_Karpovich.ViewModels
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
