using EWallet.Commands;
using EWallet.Components;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel аккаунта пользователя.
    /// </summary>
    public sealed class AccountViewModel : ViewModelBase
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
        /// Инициализирует новый экземпляр класса <see cref="AccountViewModel"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>, 
        /// хранящий данные о текущем пользователе.</param>
        /// <param name="homeNavigationService"><see cref="INavigationService"/> 
        /// совершающий переход на <see cref="HomeViewModel"/>.</param>
        /// <param name="userProfileNavigationService"><see cref="INavigationService"/> 
        /// совершающий переход на <see cref="UserProfileViewModel"/>.</param>
        /// <param name="transferNavigationService"><see cref="INavigationService"/> 
        /// совершающий переход на <see cref="TransferViewModel"/>.</param>
        /// <param name="withdrawNavigationService"><see cref="INavigationService"/> 
        /// совершающий переход на <see cref="WithdrawViewModel"/>.</param>
        /// <param name="refillNavigationService"><see cref="INavigationService"/> 
        /// совершающий переход на <see cref="RefillViewModel"/>.</param>
        /// <param name="cardManagmentNavigationService"><see cref="INavigationService"/> 
        /// совершающий переход на <see cref="CardManagmentViewModel"/>.</param>
        /// <param name="expenseReportNavigationService"><see cref="INavigationService"/> 
        /// совершающий переход на <see cref="ExpenseReportViewModel"/>.</param>
        public AccountViewModel(UserStore userStore, 
            INavigationService homeNavigationService, 
            INavigationService userProfileNavigationService,
            INavigationService transferNavigationService,
            INavigationService withdrawNavigationService,
            INavigationService refillNavigationService,
            INavigationService cardManagmentNavigationService,
            INavigationService expenseReportNavigationService)
        {
            this.userStore = userStore;

            Task.Run(FetchServices);

            NavigateTransferCommand = new NavigateCommand(transferNavigationService);
            NavigateWithdrawCommand = new NavigateCommand(withdrawNavigationService);
            NavigateRefillCommand = new NavigateCommand(refillNavigationService);
            NavigateUserProfileCommand = new NavigateCommand(userProfileNavigationService);
            NavigateManagmentCommand = new NavigateCommand(cardManagmentNavigationService);
            NavigateExpenseReportCommand = new NavigateCommand(expenseReportNavigationService);
            ExitAccountCommand = new ExitAccountCommand(userStore, homeNavigationService);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Строка для отображения баланса пользователя.
        /// </summary>
        public string StringBalance 
            => string.Format($"Баланс: {userStore.CurrentUser.Balance} руб.");
        /// <summary>
        /// Строковое отображение логина пользователя.
        /// </summary>
        public string Login 
            => userStore.CurrentUser.Login;
        /// <summary>
        /// Указывает, загружается ли список услуг в текущий момент.
        /// </summary>
        public bool AreServicesLoading
        {
            get => areServicesLoading;
            set
            {
                areServicesLoading = value;
                OnPropertyChanged(nameof(AreServicesLoading));
            }
        }
        /// <summary>
        /// Строковое представление названия услуги.
        /// </summary>        
        public string Transfer
        {
            get => transfer;
            set
            {
                transfer = value;
                OnPropertyChanged(nameof(Transfer));
            }
        }
        /// <summary>
        /// Описание услуги.
        /// </summary>
        public string TransferDescription
        {
            get => transferDescription;
            set
            {
                transferDescription = value;
                OnPropertyChanged(nameof(TransferDescription));
            }
        }
        /// <summary>
        /// Строковое представление названия услуги.
        /// </summary>   
        public string Withdraw
        {
            get => withdraw;
            set
            {
                withdraw = value;
                OnPropertyChanged(nameof(Withdraw));
            }
        }
        /// <summary>
        /// Описание услуги.
        /// </summary>
        public string WithdrawDescription
        {
            get => withdrawDescription;
            set
            {
                withdrawDescription = value;
                OnPropertyChanged(nameof(WithdrawDescription));
            }
        }
        /// <summary>
        /// Строковое представление названия услуги.
        /// </summary> 
        public string Refill
        {
            get => refill;
            set
            {
                refill = value;
                OnPropertyChanged(nameof(Refill));
            }
        }
        /// <summary>
        /// Описание услуги.
        /// </summary>
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
        /// <summary>
        /// Команда выхода из аккаунта.
        /// </summary>
        public ICommand ExitAccountCommand { get; }
        /// <summary>
        /// Команда перехода в профиль пользователя.
        /// </summary>
        public ICommand NavigateUserProfileCommand { get; }
        /// <summary>
        /// Команда перехода на страницу перевода.
        /// </summary>
        public ICommand NavigateTransferCommand { get; }
        /// <summary>
        /// Команда перехода на страницу вывода средств.
        /// </summary>
        public ICommand NavigateWithdrawCommand { get; }
        /// <summary>
        /// Команда перехода на страницу пополнения баланса.
        /// </summary>
        public ICommand NavigateRefillCommand { get; }
        /// <summary>
        /// Команда перехода на страницу управления картами.
        /// </summary>
        public ICommand NavigateManagmentCommand { get; }
        /// <summary>
        /// Команда перехода на страницу отчета о расходах.
        /// </summary>
        public ICommand NavigateExpenseReportCommand { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Получает список услуг из базы данных.
        /// </summary>
        public void FetchServices()
        {
            AreServicesLoading = true;
            try
            {
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
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally
            {
                AreServicesLoading = false;
            }
        }
        
        /// <inheritdoc cref="ViewModelBase.Dispose"/>
        public override void Dispose() 
            => base.Dispose();
        #endregion
    }
}
