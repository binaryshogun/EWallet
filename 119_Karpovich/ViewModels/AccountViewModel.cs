using _119_Karpovich.Commands;
using _119_Karpovich.Extensions;
using _119_Karpovich.Models;
using _119_Karpovich.Services;
using _119_Karpovich.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace _119_Karpovich.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        public AccountViewModel(User user, NavigationStore navigationStore)
        {
            _user = user;
            Balance = _user.Balance;
            StringBalance = string.Format($"Баланс: {Balance}");

            using (var db = new WalletEntities())
            {
                ListServices = new CollectionView(
                    new ObservableCollection<Service>(db.Service));
            }

            OpenCalculatorCommand = new OpenCalculatorCommand<Calculator>();
            ExitAccountCommand = new NavigateCommand<AuthorizationViewModel>(new NavigationService<AuthorizationViewModel>(
               navigationStore, () => new AuthorizationViewModel(navigationStore)));

            _timeNow = DateTime.Now.ToString("g");

            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _updateTimer.Tick += new EventHandler(UpdateTime);
            _updateTimer.Start();
        }

        public double Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                StringBalance = string.Format($"Баланс: {Balance}");
                OnPropertyChanged(nameof(Balance));
            }
        }

        public string StringBalance
        {
            get { return _stringBalance; }
            set
            {
                _stringBalance = value;
                OnPropertyChanged(nameof(StringBalance));
            }
        }

        public CollectionView ListServices
        {
            get { return _listServices; }
            set
            {
                _listServices = value;
                OnPropertyChanged(nameof(ListServices));
            }
        }

        public string SelectedService
        {
            get { return _selectedService; }
            set
            {
                _selectedService = value;
                OnPropertyChanged(nameof(SelectedService));
            }
        }

        public string CardNumber
        {
            get { return _cardNumber; }
            set
            {
                _cardNumber = value;
                OnPropertyChanged(nameof(CardNumber));
            }
        }

        public string OperationBalance
        {
            get { return _operationBalance; }
            set
            {
                _operationBalance = value;
                OnPropertyChanged(nameof(OperationBalance));
            }
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

        public ICommand OpenCalculatorCommand { get; }
        public ICommand DoOperationCommand { get; }
        public ICommand ExitAccountCommand { get; }

        readonly private User _user;
        private CollectionView _listServices;

        private double _balance;
        private string _selectedService;
        private string _operationBalance;
        private string _stringBalance;
        private string _cardNumber;
    }
}
