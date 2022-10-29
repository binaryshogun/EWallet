using EWallet.Commands;
using EWallet.Helpers;
using EWallet.Services;
using EWallet.Stores;
using static EWallet.Models.BanksData;
using System.Windows.Input;
using System.Windows.Media;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel страницы добавления или редактирования карт.
    /// </summary>
    public sealed class CardViewModel : ViewModelBase
    {
        #region Fields
        private readonly CardStore cardStore;

        private Banks currentBank;
        private Brush bankBackground;
        private Brush bankForeground;
        private string bankLogoPath;

        private string cardNumber;
        private string validThruMonth;
        private string validThruYear;
        private string cvv;

        private bool isDataBeingSaved;
        private bool isSaveButtonEnabled;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CardViewModel"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>,
        /// содержащий информацию о текущем пользователе.</param>
        /// <param name="cardStore"><see cref="CardStore"/>,
        /// содержащий информацию о карте при редактировании.</param>
        /// <param name="cardManagmentNavigationService">
        /// <see cref="INavigationService"/>, совершающий переход
        /// на <see cref="CardManagmentViewModel"/>.</param>
        /// <param name="accountNavigationService">
        /// <see cref="INavigationService"/>, совершающий переход
        /// на <see cref="AccountViewModel"/>.</param>
        public CardViewModel(UserStore userStore, 
			CardStore cardStore, 
			INavigationService cardManagmentNavigationService, 
			INavigationService accountNavigationService)
		{
            this.cardStore = cardStore;

            CurrentBank = Banks.Default;

            SetExistingCardData();

            NavigateCommand = new NavigateCommand(cardManagmentNavigationService);
            NavigateAccountCommand = new NavigateCommand(accountNavigationService);
            SaveCardDataCommand = new SaveCardDataCommand(userStore, this);
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
        /// Указывает, сохраняются ли данные в текущий момент.
        /// </summary>
        public bool IsDataBeingSaved
        {
            get => isDataBeingSaved;
            set
            {
                isDataBeingSaved = value;
                OnPropertyChanged(nameof(IsDataBeingSaved));
            }
        }
        /// <summary>
        /// Указывает, доступна ли кнопка сохранения данных.
        /// </summary>
        public bool IsSaveButtonEnabled
        {
            get => isSaveButtonEnabled;
            set
            {
                isSaveButtonEnabled = value;
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
            }
        }
        /// <summary>
        /// Указывает, доступны ли дополнительные поля для ввода.
        /// </summary>
        public bool AreExtraInputsAvailable => true;

        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Устанавливает существующие данные 
        /// карты при наличии свойства <see cref="CardStore.CurrentCard"/> 
        /// отличного от <see langword="null"/>.
        /// </summary>
        private void SetExistingCardData()
        {
            if (cardStore.CurrentCard != null)
            {
                CardNumber = EncryptionHelper.Decrypt(cardStore.CurrentCard.Number);
                if (cardStore.CurrentCard.ValidThru.ToString("MM")[0] == '0')
                    ValidThruMonth += cardStore.CurrentCard.ValidThru.ToString("MM")[1];
                else
                    ValidThruMonth = cardStore.CurrentCard.ValidThru.ToString("MM");
                ValidThruYear = cardStore.CurrentCard.ValidThru.ToString("yy");

                cardStore.CurrentCard = null;
            }
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
            else { CurrentBank = Banks.Default; }
        }
        /// <summary>
        /// Обновляет свойство <see cref="IsSaveButtonEnabled"/>.
        /// </summary>
        private void UpdateConfirmButton()
        {
            if (!string.IsNullOrEmpty(CardNumber) && CardNumber.Length == 16
                &&!string.IsNullOrEmpty(CVV) && !string.IsNullOrEmpty(ValidThruMonth) 
                && !string.IsNullOrEmpty(ValidThruYear))
                IsSaveButtonEnabled = true;
            else
                IsSaveButtonEnabled = false;
        }
        /// <inheritdoc cref="ViewModelBase.Dispose"/>
        public override void Dispose() 
            => base.Dispose();
        #endregion

        #region Commands
        /// <summary>
        /// Команда перехода на страницу управления картами.
        /// </summary>
        public ICommand NavigateCommand { get; }
        /// <summary>
        /// Команда перехода на страницу аккаунта пользователя.
        /// </summary>
        public ICommand NavigateAccountCommand { get; }
        /// <summary>
        /// Команда сохранения данных карты.
        /// </summary>
        public ICommand SaveCardDataCommand { get; }
        #endregion
    }
}
