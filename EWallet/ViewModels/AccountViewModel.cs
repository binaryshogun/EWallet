using EWallet.Commands;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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

        private string transfer;
        private string transferDescription;

        private string withdraw;
        private string withdrawDescription;

        private string refill;
        private string refillDescription;

        private double operationBalance;
        private string selectedService;
        private string cardNumber;

        private bool isConfirmButtonEnabled = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый объект типа AccounViewModel.
        /// </summary>
        /// <param name="user">Пользователь, прошедший авторизацию в системе.</param>
        /// <param name="navigationStore">Хранилище данных, содержащее данные о текущей ViewModel.</param>
        public AccountViewModel(UserStore userStore, INavigationService homeNavigationService, INavigationService userProfileNavigationService)
        {
            this.userStore = userStore;

            FetchServices();

            //DoOperationCommand = new DoOperationCommand(this);
            ExitAccountCommand = new ExitAccountCommand(userStore, homeNavigationService);
            NavigateUserProfileCommand = new NavigateCommand(userProfileNavigationService);
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

        /// <summary>
        /// Список доступных пользователям услуг для отображения во View.
        /// </summary>
        /// <value>
        /// Коллекция данных об услугах.
        /// </value>
        public List<Service> ListServices { get; private set; }

        //Перевод
        public string Transfer
        {
            get => ListServices[0]?.Name;
            set => transfer = value;
        }
        public string TransferDescription
        {
            get => ListServices[0]?.Caption;
            set => transferDescription = value;
        }

        //Вывод
        public string Withdraw
        {
            get => ListServices[1]?.Name;
            set => withdraw = value;
        }
        public string WithdrawDescription
        {
            get => ListServices[1]?.Caption;
            set => withdrawDescription = value;
        }

        //Пополнение баланса
        public string Refill
        {
            get => ListServices[2]?.Name;
            set => refill = value;
        }
        public string RefillDescription 
        { 
            get => ListServices[2]?.Caption; 
            set => refillDescription = value; 
        }


        /// <summary>
        /// Выбранное значение услуги в ListServices.
        /// </summary>
        /// <value>
        /// Строковое представление названия услуги.
        /// </value>
        public string SelectedService
        {
            get => selectedService;
            set
            {
                selectedService = value;
                OnPropertyChanged(nameof(SelectedService));
                if (selectedService != null && cardNumber != "" && operationBalance != 0) IsConfirmButtonEnabled = true;
            }
        }

        /// <summary>
        /// Номер карты пользователя.
        /// </summary>
        /// <value>
        /// Строковое представление номера карты пользователя.
        /// </value>
        public string CardNumber
        {
            get => cardNumber;
            set
            {
                long.TryParse(value.Replace(" ", ""), out long number);

                if (number != 0 || value != " ")
                {
                    int length = value.Replace(" ", "").Length;
                    if (length <= 4)
                        cardNumber = value;
                    else if (length <= 8)
                        cardNumber = string.Format($"{number:#### ####}");
                    else if (length <= 12)
                        cardNumber = string.Format($"{number:#### #### ####}");
                    else
                        cardNumber = string.Format($"{number:#### #### #### ####}");
                }

                if (cardNumber != null)
                {
                    cardNumber.Trim(' ');
                }

                OnPropertyChanged(nameof(CardNumber));
                if (cardNumber != "" && operationBalance != 0 && selectedService != null)
                    IsConfirmButtonEnabled = true;
            }
        }

        /// <summary>
        /// Баланс операции.
        /// </summary>
        /// <value>
        /// Действительное число с двойной точностью, содержащее баланс операции.
        /// </value>
        public double OperationBalance
        {
            get => operationBalance;
            set
            {
                if (operationBalance != value)
                {
                    operationBalance = value;
                    OnPropertyChanged(nameof(OperationBalance));
                    if (cardNumber != "" && operationBalance != 0 && selectedService != null)
                        IsConfirmButtonEnabled = true;
                }
            }
        }

        /// <summary>
        /// Флаг, отвечающий за включение и отключение кнопки проведения операции.
        /// </summary>
        /// <value>
        /// Булева переменная, содержащая состояние кнопки проведения операции.
        /// </value>
        public bool IsConfirmButtonEnabled
        {
            get => isConfirmButtonEnabled;
            set
            {
                isConfirmButtonEnabled = value;
                OnPropertyChanged(nameof(IsConfirmButtonEnabled));
            }
        }
        #endregion

        #region Commands
        public ICommand ExitAccountCommand { get; }
        public ICommand NavigateUserProfileCommand { get; }
        public ICommand ReportExpensesCommand { get; }
        #endregion

        #region Methods
        public void FetchServices()
        {
            using (var dataBase = new WalletEntities())
            {
                ListServices = dataBase.Service.AsNoTracking().Select(s => s).ToList();
            }
        }
        
        public override void Dispose() 
            => base.Dispose();
        #endregion
    }
}
