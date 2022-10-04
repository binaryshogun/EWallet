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
        private int serialNumber;
        private int number;
        private int divisionCode;

        private bool doesUserHavePatronymic;
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

            NavigateCommand = new NavigateCommand(accountNavigationService);
            SaveCommand = new SavePassportDataCommand(this, userStore);
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

        public int SerialNumber
        {
            get => serialNumber;
            set
            {
                serialNumber = value;
                OnPropertyChanged(nameof(SerialNumber));
            }
        }

        public int Number
        {
            get => number;
            set 
            { 
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        public int DivisionCode
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
                doesUserHavePatronymic = value;
                OnPropertyChanged(nameof(DoesUserHavePatronymic));
            }
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand { get; }
        public ICommand SaveCommand { get; }
        #endregion
    }
}
