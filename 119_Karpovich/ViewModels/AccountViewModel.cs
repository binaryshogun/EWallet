using _119_Karpovich.Commands;
using _119_Karpovich.Models;
using _119_Karpovich.Services;
using _119_Karpovich.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace _119_Karpovich.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        public AccountViewModel(User user, NavigationStore navigationStore)
        {
            ExitAccountCommand = new NavigateCommand<AuthorizationViewModel>(new NavigationService<AuthorizationViewModel>(
                navigationStore, () => new AuthorizationViewModel(navigationStore)));

            _user = user;
            Balance = _user.Balance;
            StringBalance = string.Format($"Баланс: {Balance}");

            using (var db = new WalletEntities())
            {

            }
        }

        public double Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                OnPropertyChanged(nameof(Balance));
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

        public string StringBalance
        {
            get { return _stringBalance; }
            set
            {
                _stringBalance = value;
                OnPropertyChanged(nameof(StringBalance));
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

        public string SelectedService
        {
            get { return _selectedService; }
            set
            {
                _selectedService = value;
                OnPropertyChanged(nameof(SelectedService));
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
