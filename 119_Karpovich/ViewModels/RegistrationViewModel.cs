using _119_Karpovich.Commands;
using _119_Karpovich.Stores;
using _119_Karpovich.Services;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace _119_Karpovich.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        public RegistrationViewModel(NavigationStore navigationStore)
        {
            NavigateCommand = new NavigateCommand<AuthorizationViewModel>(new NavigationService<AuthorizationViewModel>(
                navigationStore, () => new AuthorizationViewModel(navigationStore)));
            RegisterUserCommand = new RegisterUserCommand(this, new NavigationService<AuthorizationViewModel>(
                navigationStore, () => new AuthorizationViewModel(navigationStore)));

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
                IsRegistrationButtonEnabled = EnableRegistrationButton();
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
                IsRegistrationButtonEnabled = EnableRegistrationButton();
            }
        }

        private string repeatedPassword = "";
        public string RepeatedPassword
        {
            get { return repeatedPassword; }
            set 
            { 
                repeatedPassword = value;
                OnPropertyChanged(nameof(RepeatedPassword));
                IsRegistrationButtonEnabled = EnableRegistrationButton();
            }
        }

        private bool isRegistrationButtonEnabled;
        public bool IsRegistrationButtonEnabled
        {
            get { return isRegistrationButtonEnabled; }
            set
            {
                isRegistrationButtonEnabled = value;
                OnPropertyChanged(nameof(IsRegistrationButtonEnabled));
            }
        }

        private bool EnableRegistrationButton()
        {
            if (login != "" 
                && password != "" 
                && repeatedPassword != "" 
                && password == repeatedPassword)
                return true;
            else
                return false;
        }

        public ICommand RegisterUserCommand { get; }
        public ICommand NavigateCommand { get; }
    }
}
