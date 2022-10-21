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

namespace EWallet.ViewModels
{
    public class RefillViewModel : ViewModelBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly double percent;

        private Banks currentBank;
        private Brush bankBorderBrush;
        private Brush bankBackground;
        private Brush bankForeground;
        private string bankName;

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
        public RefillViewModel(UserStore userStore, INavigationService accountNavigationService)
        {
            this.userStore = userStore;
            BankBorderBrush = new SolidColorBrush(Color.FromRgb(77, 39, 97));
            BankBackground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            BankForeground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            BankName = "БАНК";
            Comission = "0,00";

            try
            {
                using (var database = new WalletEntities())
                {
                    SetExistingCardData(database);
                    var service = database.Service.AsNoTracking().Where(s => s.ID == 4).First();
                    double.TryParse(service.Comission.ToString(), out percent);
                }
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally
            {
                ProvideOperationCommand = new RefillCommand(userStore, this, accountNavigationService);
                NavigateAccountCommand = new NavigateCommand(accountNavigationService);
            }
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

                if (cardNumber.Length > 0)
                {
                    IdentifyBank();

                    OnPropertyChanged(nameof(CurrentBank));
                }

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
                OnPropertyChanged(nameof(ValidThruMonth));
            }
        }
        public string ValidThruYear
        {
            get => validThruYear;
            set
            {
                validThruYear = value;
                OnPropertyChanged(nameof(ValidThruYear));
            }
        }
        public string CVV
        {
            get => cvv;
            set
            {
                cvv = value;
                OnPropertyChanged(nameof(CVV));
            }
        }
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
        private Banks CurrentBank
        {
            set
            {
                currentBank = value;

                BankBorderBrush = BankBorderColors[currentBank];
                BankBackground = BankBorderColors[currentBank];
                BankForeground = BankForegrounds[currentBank];
                BankName = BankNames[currentBank];
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
        public string BankName
        {
            get => bankName;
            set
            {
                bankName = value;
                OnPropertyChanged(nameof(BankName));
            }
        }
        #endregion

        #region VisualProperties
        public string UserBalance
            => userStore.CurrentUser.Balance.ToString();
        public string ButtonContent => "Пополнить";
        public bool IsOperationBeingProvided
        {
            get => isOperationBeingProvided;
            set
            {
                isOperationBeingProvided = value;
                OnPropertyChanged(nameof(IsOperationBeingProvided));
            }
        }
        public bool OperationWithTransfer => false;
        public bool IsOperation => true;
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

        #endregion

        #region Methods
        private void SetExistingCardData(WalletEntities database)
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

        private void IdentifyBank()
        {
            char firstDigit = cardNumber[0];
            if (firstDigit > '0' && firstDigit < '4')
                CurrentBank = Banks.Sberbank;
            else if (firstDigit >= '4' && firstDigit < '7')
                CurrentBank = Banks.AlfaBank;
            else if (firstDigit >= '7' && firstDigit <= '9')
                CurrentBank = Banks.Tinkoff;
        }

        private string GetComission()
        {
            double.TryParse(OperationSum, out double sum);
            return string.Format("{0:f2}", sum * percent);
        }

        private void UpdateConfirmButton()
        {
            if (!string.IsNullOrEmpty(CardNumber) && CardNumber.Length == 16 
                && !string.IsNullOrEmpty(OperationSum) && !string.IsNullOrEmpty(CVV)
                && !string.IsNullOrEmpty(ValidThruMonth) && !string.IsNullOrEmpty(ValidThruYear))
                IsConfirmButtonEnabled = true;
            else
                IsConfirmButtonEnabled = false;
        }
        #endregion

        #region Commands
        public ICommand ProvideOperationCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        #endregion
    }
}
