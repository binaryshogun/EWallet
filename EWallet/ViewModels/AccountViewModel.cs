using EWallet.Commands;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel аккаунта пользователя.
    /// </summary>
    public class AccountViewModel : ViewModelBase
    {
        #region Fields
        public readonly UserStore userStore;

        //services
        private bool areServicesLoading;
        private string transfer;
        private string transferDescription;
        private string withdraw;
        private string withdrawDescription;
        private string refill;
        private string refillDescription;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый объект типа AccounViewModel.
        /// </summary>
        /// <param name="user">Пользователь, прошедший авторизацию в системе.</param>
        /// <param name="navigationStore">Хранилище данных, содержащее данные о текущей ViewModel.</param>
        public AccountViewModel(UserStore userStore, 
            INavigationService homeNavigationService, 
            INavigationService userProfileNavigationService,
            INavigationService transferNavigationService,
            INavigationService withdrawNavigationService,
            INavigationService refillNavigationService)
        {
            this.userStore = userStore;

            Task.Run(FetchServices);

            NavigateTransferCommand = new NavigateCommand(transferNavigationService);
            NavigateWithdrawCommand = new NavigateCommand(withdrawNavigationService);
            NavigateRefillCommand = new NavigateCommand(refillNavigationService);
            NavigateUserProfileCommand = new NavigateCommand(userProfileNavigationService);
            ExitAccountCommand = new ExitAccountCommand(userStore, homeNavigationService);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Строка отображения баланса пользователя во View.
        /// </summary>
        /// <value>
        /// Строковое представление баланса пользователя.
        /// </value>
        public string StringBalance 
            => string.Format($"Баланс: {userStore.CurrentUser.Balance} руб.");

        public string Login 
            => userStore.CurrentUser.Login;

        public bool AreServicesLoading
        {
            get => areServicesLoading;
            set
            {
                areServicesLoading = value;
                OnPropertyChanged(nameof(AreServicesLoading));
            }
        }

        //Перевод
        public string Transfer
        {
            get => transfer;
            set
            {
                transfer = value;
                OnPropertyChanged(nameof(Transfer));
            }
        }
        public string TransferDescription
        {
            get => transferDescription;
            set
            {
                transferDescription = value;
                OnPropertyChanged(nameof(TransferDescription));
            }
        }

        //Вывод
        public string Withdraw
        {
            get => withdraw;
            set
            {
                withdraw = value;
                OnPropertyChanged(nameof(Withdraw));
            }
        }
        public string WithdrawDescription
        {
            get => withdrawDescription;
            set
            {
                withdrawDescription = value;
                OnPropertyChanged(nameof(WithdrawDescription));
            }
        }

        //Пополнение баланса
        public string Refill
        {
            get => refill;
            set
            {
                refill = value;
                OnPropertyChanged(nameof(Refill));
            }
        }
        public string RefillDescription
        {
            get => refillDescription;
            set
            {
                refillDescription = value;
                OnPropertyChanged(nameof(RefillDescription));
            }
        }
        #endregion

        #region Commands
        public ICommand ExitAccountCommand { get; }
        public ICommand NavigateUserProfileCommand { get; }
        public ICommand NavigateTransferCommand { get; }
        public ICommand NavigateWithdrawCommand { get; }
        public ICommand NavigateRefillCommand { get; }
        public ICommand ReportExpensesCommand { get; }
        #endregion

        #region Methods
        public void FetchServices()
        {
            AreServicesLoading = true;
            using (var dataBase = new WalletEntities())
            {
                List<Service> ListServices = dataBase.Service.AsNoTracking().Select(s => s).ToList();
                Transfer = ListServices[0]?.Name;
                TransferDescription = ListServices[0]?.Caption;
                Withdraw = ListServices[1]?.Name;
                WithdrawDescription = ListServices[1]?.Caption;
                Refill = ListServices[2]?.Name;
                RefillDescription = ListServices[2]?.Caption;
            }
            AreServicesLoading = false;
        }
        
        public override void Dispose() 
            => base.Dispose();
        #endregion
    }
}
