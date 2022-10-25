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
    public class RefillViewModel : ViewModelBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly double percent;

        private Banks currentBank;
        private Brush bankBorderBrush;
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
        public RefillViewModel(UserStore userStore, INavigationService accountNavigationService)
        {
            this.userStore = userStore;

            CurrentBank = Banks.Default;
            Comission = "0,00";

            ProvideOperationCommand = new RefillCommand(userStore, this, accountNavigationService);
            NavigateAccountCommand = new NavigateCommand(accountNavigationService);

            try
            {
                SetExistingCardData();
                
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
        public bool IsOperationBeingProvided
        {
            get => isOperationBeingProvided;
            set
            {
                isOperationBeingProvided = value;
                OnPropertyChanged(nameof(IsOperationBeingProvided));
            }
        }
        public bool IsConfirmButtonEnabled
        {
            get => isConfirmButtonEnabled;
            set
            {
                isConfirmButtonEnabled = value;
                OnPropertyChanged(nameof(IsConfirmButtonEnabled));
            }
        }
        public string UserBalance => userStore.CurrentUser.Balance.ToString();
        public string ButtonContent => "Пополнить";
        public bool AreExtraInputsAvailable => true;
        
        #endregion

        #endregion

        #region Methods
        private void SetExistingCardData() =>
            Task.Run(FetchCardData);

        private void FetchCardData()
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
