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

            _timeNow = DateTime.Now.ToString("g");

            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _updateTimer.Tick += new EventHandler(UpdateTime);
            _updateTimer.Start();
        }

        private readonly DispatcherTimer _updateTimer;
        private void UpdateTime(object sender, EventArgs e)
        {
            TimeNow = DateTime.Now.ToString("g");
        }


        private string _timeNow;
        public string TimeNow
        {
            get { return _timeNow; }
            set
            {
                _timeNow = value;
                OnPropertyChanged(nameof(TimeNow));
            }
        }

        private string _login = "";
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
                IsRegistrationButtonEnabled = EnableRegistrationButton();
            }
        }

        private string _password = "";
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                IsRegistrationButtonEnabled = EnableRegistrationButton();
            }
        }

        private string _repeatedPassword = "";

        public string RepeatedPassword
        {
            get { return _repeatedPassword; }
            set 
            { 
                _repeatedPassword = value;
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
            if (_login != "" 
                && _password != "" 
                && _repeatedPassword != "" 
                && _password == _repeatedPassword)
                return true;
            else
                return false;
        }

        public ICommand RegisterUserCommand { get; }
        public ICommand NavigateCommand { get; }
    }
}
