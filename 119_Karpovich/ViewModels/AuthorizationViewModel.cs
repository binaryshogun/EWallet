using _119_Karpovich.Commands;
using _119_Karpovich.Services;
using _119_Karpovich.Stores;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace _119_Karpovich.ViewModels
{
    public class AuthorizationViewModel : ViewModelBase
    {
        public AuthorizationViewModel(NavigationStore navigationStore)
        {
            NavigateCommand = new NavigateCommand<RegistrationViewModel>(new NavigationService<RegistrationViewModel>(
                navigationStore, () => new RegistrationViewModel(navigationStore)));
            AuthorizeUserCommand = new AuthorizeUserCommand(this, navigationStore);

            timeNow = DateTime.Now.ToString("g");

            updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            updateTimer.Tick += new EventHandler(UpdateTime);
            updateTimer.Start();
        }

        private readonly DispatcherTimer updateTimer;
        private void UpdateTime(object sender, EventArgs e)
        {
            TimeNow = DateTime.Now.ToString("g");
        }


        private string timeNow;
        public string TimeNow
        {
            get { return timeNow; }
            set
            {
                timeNow = value;
                OnPropertyChanged(nameof(TimeNow));
            }
        }

        private string login = "";
        public string Login
        {
            get { return login; }
            set 
            { 
                login = value;
                OnPropertyChanged(nameof(Login));
                IsEnterButtonEnabled = EnableEnterButton();
            }
        }


        private string password = "";
        public string Password
        {
            get { return password; }
            set 
            { 
                password = value;
                OnPropertyChanged(nameof(Password));
                IsEnterButtonEnabled = EnableEnterButton();
            }
        }

        private bool isEnterButtonEnabled = false;

        public bool IsEnterButtonEnabled
        {
            get { return isEnterButtonEnabled; }
            set 
            { 
                isEnterButtonEnabled = value;
                OnPropertyChanged(nameof(IsEnterButtonEnabled));
            }
        }

        private bool EnableEnterButton()
        {
            if (login != "" && password != "")
                return true;
            else 
                return false;
        }

        public ICommand NavigateCommand { get; }
        public ICommand AuthorizeUserCommand { get; }
    }
}
