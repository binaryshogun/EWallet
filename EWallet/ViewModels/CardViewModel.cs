using EWallet.Commands;
using EWallet.Helpers;
using EWallet.Services;
using EWallet.Stores;
using static EWallet.Models.BanksData;
using System.Windows.Input;
using System.Windows.Media;

namespace EWallet.ViewModels
{
    public sealed class CardViewModel : ViewModelBase
    {
        #region Fields
        private readonly CardStore cardStore;

        private Banks currentBank;
        private Brush bankBorderBrush;
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
        private Banks CurrentBank
        {
            set
            {
                currentBank = value;

                BankBorderBrush = BankColors[currentBank];
                BankBackground = BankColors[currentBank];
                BankForeground = BankForegrounds[currentBank];
                BankLogoPath = BankLogos[currentBank];

                OnPropertyChanged(nameof(CurrentBank));
            }
        }
        public Brush BankBorderBrush
        {
            get => bankBorderBrush;
            set
            {
                bankBorderBrush = value;
                OnPropertyChanged(nameof(BankBorderBrush));
            }
        }
        public Brush BankBackground
        {
            get => bankBackground;
            set
            {
                bankBackground = value;
                OnPropertyChanged(nameof(BankBackground));
            }
        }
        public Brush BankForeground
        {
            get => bankForeground;
            set
            {
                bankForeground = value;
                OnPropertyChanged(nameof(BankForeground));
            }
        }
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
        public bool IsDataBeingSaved
        {
            get => isDataBeingSaved;
            set
            {
                isDataBeingSaved = value;
                OnPropertyChanged(nameof(IsDataBeingSaved));
            }
        }
        public bool IsSaveButtonEnabled
        {
            get => isSaveButtonEnabled;
            set
            {
                isSaveButtonEnabled = value;
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
            }
        }
        public bool AreExtraInputsAvailable => true;

        #endregion
        #endregion

        #region Methods
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

        private void UpdateConfirmButton()
        {
            if (!string.IsNullOrEmpty(CardNumber) && CardNumber.Length == 16
                &&!string.IsNullOrEmpty(CVV) && !string.IsNullOrEmpty(ValidThruMonth) 
                && !string.IsNullOrEmpty(ValidThruYear))
                IsSaveButtonEnabled = true;
            else
                IsSaveButtonEnabled = false;
        }

        public override void Dispose() 
            => base.Dispose();
        #endregion

        #region Commands
        public ICommand NavigateCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand SaveCardDataCommand { get; }
        #endregion
    }
}
