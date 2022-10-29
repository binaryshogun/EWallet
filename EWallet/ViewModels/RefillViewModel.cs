using EWallet.Commands;
using EWallet.Components;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using System;
using System.Linq;
using static EWallet.Models.BanksData;
using System.Windows.Input;
using System.Windows.Media;
using EWallet.Helpers;
using System.Threading.Tasks;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel страницы пополнения баланса.
    /// </summary>
    public sealed class RefillViewModel : ViewModelBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly double percent;

        private Banks currentBank;
        private Brush bankBackground;
        private Brush bankForeground;
        private string bankLogoPath;

        private string cardNumber;
        private string validThruMonth;
        private string validThruYear;
        private string cvv;
        private bool saveCardData;

        private string operationSum;
        private string comission;

        private bool isOperationBeingProvided;
        private bool isConfirmButtonEnabled;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RefillViewModel"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>,
        /// содержащий данные о текущем пользователе.</param>
        /// <param name="accountNavigationService"><see cref="INavigationService"/>,
        /// совершающий переход на <see cref="AccountViewModel"/>.</param>
        public RefillViewModel(UserStore userStore, INavigationService accountNavigationService)
        {
            this.userStore = userStore;

            CurrentBank = Banks.Default;
            Comission = "0,00";

            ProvideOperationCommand = new RefillCommand(userStore, this, accountNavigationService);
            NavigateAccountCommand = new NavigateCommand(accountNavigationService);

            try
            {
                Task.Run(SetExistingCardData);
                
                using (var database = new WalletEntities())
                {
                    var service = database.Service.First(s => s.Name == "Пополнение баланса");
                    double.TryParse(service.Comission.ToString(), out percent);
                }
            }
            catch (Exception e) 
            { 
                ErrorMessageBox.Show(e);
                accountNavigationService?.Navigate();
            }
        }
        #endregion

        #region Properties
        #region CardData
        /// <summary>
        /// Номер карты.
        /// </summary>
        public string CardNumber
        {
            get => cardNumber;
            set
            {
                cardNumber = value;

                IdentifyBank();
                UpdateConfirmButton();

                OnPropertyChanged(nameof(CardNumber));
            }
        }
        /// <summary>
        /// Месяц срока годности карты.
        /// </summary>
        public string ValidThruMonth
        {
            get => validThruMonth;
            set
            {
                validThruMonth = value;

                UpdateConfirmButton();
                OnPropertyChanged(nameof(ValidThruMonth));
            }
        }
        /// <summary>
        /// Год срока годности карты.
        /// </summary>
        public string ValidThruYear
        {
            get => validThruYear;
            set
            {
                validThruYear = value;

                UpdateConfirmButton();
                OnPropertyChanged(nameof(ValidThruYear));
            }
        }
        /// <summary>
        /// Секретный код карты.
        /// </summary>
        public string CVV
        {
            get => cvv;
            set
            {
                cvv = value;

                UpdateConfirmButton();
                OnPropertyChanged(nameof(CVV));
            }
        }
        /// <summary>
        /// Указывает, необходимо ли сохранять данные карты.
        /// </summary>
        public bool SaveCardData
        {
            get => saveCardData;
            set
            {
                saveCardData = value;
                OnPropertyChanged(nameof(SaveCardData));
            }
        }
        #endregion

        #region OperationData
        /// <summary>
        /// Сумма операции.
        /// </summary>
        public string OperationSum
        {
            get => operationSum;
            set
            {
                operationSum = value;

                Comission = GetComission();
                UpdateConfirmButton();

                OnPropertyChanged(nameof(OperationSum));
            }
        }
        /// <summary>
        /// Комиссионный взнос за проведение операции.
        /// </summary>
        public string Comission
        {
            get => comission;
            set
            {
                comission = value;
                OnPropertyChanged(nameof(Comission));
            }
        }
        #endregion

        #region BankData
        /// <summary>
        /// Текущий банк, определяющийся по номеру карты.
        /// </summary>
        private Banks CurrentBank
        {
            set
            {
                currentBank = value;

                BankBackground = BankColors[currentBank];
                BankForeground = BankForegrounds[currentBank];
                BankLogoPath = BankLogos[currentBank];

                OnPropertyChanged(nameof(CurrentBank));
            }
        }
        /// <summary>
        /// Цвет фона банка.
        /// </summary>
        public Brush BankBackground
        {
            get => bankBackground;
            set
            {
                bankBackground = value;
                OnPropertyChanged(nameof(BankBackground));
            }
        }
        /// <summary>
        /// Цвет шрифта банка.
        /// </summary>
        public Brush BankForeground
        {
            get => bankForeground;
            set
            {
                bankForeground = value;
                OnPropertyChanged(nameof(BankForeground));
            }
        }
        /// <summary>
        /// Путь к логотипу банка.
        /// </summary>
        public string BankLogoPath
        {
            get => bankLogoPath;
            set
            {
                bankLogoPath = value;
                OnPropertyChanged(nameof(BankLogoPath));
            }
        }
        #endregion

        #region VisualProperties
        /// <summary>
        /// Указывает, проводится ли операция в текущий момент времени.
        /// </summary>
        public bool IsOperationBeingProvided
        {
            get => isOperationBeingProvided;
            set
            {
                isOperationBeingProvided = value;
                OnPropertyChanged(nameof(IsOperationBeingProvided));
            }
        }
        /// <summary>
        /// Указывает, доступна ли кнопка проведения операции.
        /// </summary>
        public bool IsConfirmButtonEnabled
        {
            get => isConfirmButtonEnabled;
            set
            {
                isConfirmButtonEnabled = value;
                OnPropertyChanged(nameof(IsConfirmButtonEnabled));
            }
        }
        /// <summary>
        /// Строка текущего баланса пользователя.
        /// </summary>
        public string UserBalance => userStore.CurrentUser.Balance.ToString();
        /// <summary>
        /// Содержание кнопки проведения операции.
        /// </summary>
        public string ButtonContent => "Пополнить";
        /// <summary>
        /// Указывает, доступны ли дополнительные поля ввода.
        /// </summary>
        public bool AreExtraInputsAvailable => true;
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Устанавливает сохраненной карты в поля ввода.
        /// </summary>
        private void SetExistingCardData()
        {
            try
            {
                using (var database = new WalletEntities())
                {
                    var card = database.Card.AsNoTracking().Where(c => c.UserID == userStore.CurrentUser.ID).FirstOrDefault();
                    if (card != null)
                    {
                        CardNumber = EncryptionHelper.Decrypt(card.Number);
                        if (card.ValidThru.ToString("MM")[0] == '0')
                            ValidThruMonth += card.ValidThru.ToString("MM")[1];
                        else
                            ValidThruMonth = card.ValidThru.ToString("MM");
                        ValidThruYear = card.ValidThru.ToString("yy");
                        SaveCardData = true;
                    }
                }
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
        }
        /// <summary>
        /// Идентифицирует банк по номеру карты.
        /// </summary>
        private void IdentifyBank()
        {
            if (CardNumber.Length > 0)
            {

                char firstDigit = CardNumber[0];
                if (firstDigit > '0' && firstDigit < '4')
                    CurrentBank = Banks.Sberbank;
                else if (firstDigit >= '4' && firstDigit < '7')
                    CurrentBank = Banks.AlfaBank;
                else if (firstDigit >= '7' && firstDigit <= '9')
                    CurrentBank = Banks.Tinkoff;
            }
            else
                CurrentBank = Banks.Default;
        }
        /// <summary>
        /// Рассчитывает комиссию за операцию.
        /// </summary>
        /// <returns>Строковое представление комиссии за операцию.</returns>
        private string GetComission()
        {
            double.TryParse(OperationSum, out double sum);
            return string.Format("{0:f2}", sum * percent);
        }
        /// <summary>
        /// Обновляет свойство <see cref="IsConfirmButtonEnabled"/>.
        /// </summary>
        private void UpdateConfirmButton()
        {
            if (!string.IsNullOrEmpty(CardNumber) && CardNumber.Length == 16 
                && !string.IsNullOrEmpty(OperationSum) && !string.IsNullOrEmpty(CVV)
                && !string.IsNullOrEmpty(ValidThruMonth) && !string.IsNullOrEmpty(ValidThruYear))
                IsConfirmButtonEnabled = true;
            else
                IsConfirmButtonEnabled = false;
        }
        /// <inheritdoc cref="ViewModelBase.Dispose"/>
        public override void Dispose() 
            => base.Dispose();
        #endregion

        #region Commands
        /// <summary>
        /// Команда проведения операции.
        /// </summary>
        public ICommand ProvideOperationCommand { get; }
        /// <summary>
        /// Команда перехода на страницу аккаунта пользователя.
        /// </summary>
        public ICommand NavigateAccountCommand { get; }
        #endregion
    }
}
