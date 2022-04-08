using _119_Karpovich.Commands;
using _119_Karpovich.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace _119_Karpovich.ViewModels
{
    internal class AuthorizationViewModel : ViewModelBase
    {
        public AuthorizationViewModel(NavigationStore navigationStore)
        {
            NavigateCommand = new NavigateCommand<RegistrationViewModel>(navigationStore, () => new RegistrationViewModel(navigationStore));

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

        private string _password;
        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand NavigateCommand { get; }
    }
}
