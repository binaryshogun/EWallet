using _119_Karpovich.Commands;
using _119_Karpovich.Extensions;
using _119_Karpovich.Models;
using _119_Karpovich.Services;
using _119_Karpovich.Stores;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace _119_Karpovich.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        public AccountViewModel(User user, NavigationStore navigationStore)
        {
            this.user = user;
            Balance = user.Balance;
            StringBalance = string.Format($"Баланс: {Balance}");

            using (var db = new WalletEntities())
            {
                ListServices = new CollectionView(
                    new ObservableCollection<Service>(db.Service));
            }

            DoOperationCommand = new DoOperationCommand(this);
            OpenCalculatorCommand = new OpenCalculatorCommand<Calculator>();
            ExitAccountCommand = new ExitAccountCommand<AuthorizationViewModel>(new NavigationService<AuthorizationViewModel>(
               navigationStore, () => new AuthorizationViewModel(navigationStore)));

            timeNow = DateTime.Now.ToString("g");

            updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            updateTimer.Tick += new EventHandler(UpdateTime);
            updateTimer.Start();
        }

        public double Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                StringBalance = string.Format($"Баланс: {Balance}");
                OnPropertyChanged(nameof(Balance));
            }
        }

        public string StringBalance
        {
            get { return stringBalance; }
            set
            {
                stringBalance = value;
                OnPropertyChanged(nameof(StringBalance));
            }
        }

        public CollectionView ListServices
        {
            get { return listServices; }
            set
            {
                listServices = value;
                OnPropertyChanged(nameof(ListServices));
            }
        }

        public string SelectedService
        {
            get { return selectedService; }
            set
            {
                selectedService = value;
                OnPropertyChanged(nameof(SelectedService));
                if (selectedService != null && cardNumber != "" && operationBalance != 0) IsConfirmButtonEnabled = true;
            }
        }

        public string CardNumber
        {
            get 
            {
                return cardNumber; 
            }
            set
            {
                long number;
                long.TryParse(value.Replace(" ", ""), out number);

                if (value.Length <= 4)
                    cardNumber = value;
                else if (value.Length <= 8)
                    cardNumber = string.Format($"{number:#### ####}");
                else if (value.Length <= 12)
                    cardNumber = string.Format($"{number:#### #### ####}");
                else
                    cardNumber = string.Format($"{number:#### #### #### ####}");

                OnPropertyChanged(nameof(CardNumber));
                if (cardNumber != "" && operationBalance != 0 && selectedService != null) IsConfirmButtonEnabled = true;
            }
        }

        public double OperationBalance
        {
            get { return operationBalance; }
            set
            {
                operationBalance = value;
                OnPropertyChanged(nameof(OperationBalance));
                if (cardNumber != "" && operationBalance != 0 && selectedService != null) IsConfirmButtonEnabled = true;
            }
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

        private bool isConfirmButtonEnabled = false;

        public bool IsConfirmButtonEnabled
        {
            get { return isConfirmButtonEnabled; }
            set
            { 
                isConfirmButtonEnabled = value;
                OnPropertyChanged(nameof(IsConfirmButtonEnabled));
            }
        }


        public ICommand OpenCalculatorCommand { get; }
        public ICommand DoOperationCommand { get; }
        public ICommand ExitAccountCommand { get; }

        readonly public User user;
        private CollectionView listServices;
        private WalletEntities db;

        private double balance;
        private double operationBalance;
        private string selectedService;
        private string stringBalance;
        private string cardNumber;
    }
}
