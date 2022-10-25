using EWallet.Commands;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using System.Windows.Input;
using System.Linq;

namespace EWallet.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
        #region Fields
        private readonly Passport passport;

        private string firstName;
        private string lastName;
        private string patronymic;
        private string saveDataMessage;
        private string serialNumber;
        private string number;
        private string divisionCode;

        private bool isDataSave;
        private bool isDataSaved;
        #endregion

        #region Constructors
        public UserProfileViewModel(UserStore userStore, 
            INavigationService accountNavigationService)
        {

            using (var dataBase = new WalletEntities())
            {
                passport = dataBase.Passport.AsNoTracking().
                    Where(p => p.UserID == userStore.CurrentUser.ID).FirstOrDefault();
            }

            if (passport != null)
            {
                FirstName = passport.FirstName;
                LastName = passport.LastName;
                Patronymic = passport.Patronymic;
                SerialNumber = passport.SerialNumber;
                Number = passport.Number;
                DivisionCode = passport.DivisionCode;
            }

            IsDataSaved = true;

            NavigateAccountCommand = new NavigateCommand(accountNavigationService);
            SaveCommand = new SavePassportDataCommand(userStore, this);
        }
        #endregion

        #region Properties
        public string FirstName
        {
            get => firstName;
            set 
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => lastName;
            set 
            { 
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Patronymic
        {
            get => patronymic;
            set 
            {
                patronymic = value;
                OnPropertyChanged(nameof(Patronymic));
            }
        }

        public string SaveDataMessage
        {
            get => saveDataMessage;
            set
            {
                saveDataMessage = value;
                OnPropertyChanged(nameof(SaveDataMessage));
            }
        }

        public string SerialNumber
        {
            get => serialNumber;
            set
            {
                serialNumber = value;
                OnPropertyChanged(nameof(SerialNumber));
            }
        }

        public string Number
        {
            get => number;
            set 
            { 
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        public string DivisionCode
        {
            get => divisionCode;
            set 
            { 
                divisionCode = value;
                OnPropertyChanged(nameof(DivisionCode));
            }
        }

        public bool DoesUserHavePatronymic
        {
            get => Patronymic != null;
            set 
            {
                Patronymic = value ? "" : null;
                OnPropertyChanged(nameof(DoesUserHavePatronymic));
            }
        }

        public bool IsDataSave
        {
            get => isDataSave;
            set
            {
                isDataSave = value;
                OnPropertyChanged(nameof(IsDataSave));
            }
        }
        public bool IsDataSaved
        {
            get => isDataSaved;
            set
            {
                isDataSaved = value;
                OnPropertyChanged(nameof(IsDataSaved));
            }
        }
        #endregion

        #region Commands
        public ICommand NavigateAccountCommand { get; }
        public ICommand SaveCommand { get; }
        #endregion
    }
}
