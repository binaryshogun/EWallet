using _119_Karpovich.Commands;
using _119_Karpovich.Stores;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace _119_Karpovich.ViewModels
{
    internal class RegistrationViewModel : ViewModelBase
    {
        public RegistrationViewModel(NavigationStore navigationStore)
        {
            NavigateCommand = new NavigateCommand<AuthorizationViewModel>(navigationStore, () => new AuthorizationViewModel(navigationStore));

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

        private string _repeatedPassword;

        public string RepeatedPassword
        {
            get { return _repeatedPassword; }
            set { _repeatedPassword = value; }
        }

        public ICommand NavigateCommand { get; }
    }

    public class IsEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("IsEmptyConverter can only be used OneWay.");
        }
    }
}
