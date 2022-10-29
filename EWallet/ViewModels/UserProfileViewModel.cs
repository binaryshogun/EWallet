using EWallet.Commands;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using System.Windows.Input;
using System.Linq;
using EWallet.Components;
using System;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel для страницы профиля пользователя.
    /// </summary>
    public sealed class UserProfileViewModel : ViewModelBase
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
        /// <summary>
        /// Инициализирует новый экземпляр 
        /// класса <see cref="UserProfileViewModel"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>,
        /// хранящий данные о текущем пользователе.</param>
        /// <param name="accountNavigationService">
        /// <see cref="INavigationService"/>, совершающий
        /// переход на <see cref="AccountViewModel"/>.</param>
        public UserProfileViewModel(UserStore userStore, 
            INavigationService accountNavigationService)
        {
            try
            {
                using (var dataBase = new WalletEntities())
                {
                    passport = dataBase.Passport.AsNoTracking().
                        Where(p => p.UserID == userStore.CurrentUser.ID).FirstOrDefault();
                }
            }
            catch (Exception e) 
            { 
                ErrorMessageBox.Show(e); 
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
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FirstName
        {
            get => firstName;
            set 
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string LastName
        {
            get => lastName;
            set 
            { 
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        /// <summary>
        /// Отчество пользователя.
        /// </summary>
        public string Patronymic
        {
            get => patronymic;
            set 
            {
                patronymic = value;
                OnPropertyChanged(nameof(Patronymic));
            }
        }
        /// <summary>
        /// Сообщает пользователю о 
        /// статусе сохранения.
        /// </summary>
        public string SaveDataMessage
        {
            get => saveDataMessage;
            set
            {
                saveDataMessage = value;
                OnPropertyChanged(nameof(SaveDataMessage));
            }
        }
        /// <summary>
        /// Серия паспорта.
        /// </summary>
        public string SerialNumber
        {
            get => serialNumber;
            set
            {
                serialNumber = value;
                OnPropertyChanged(nameof(SerialNumber));
            }
        }
        /// <summary>
        /// Номер паспорта.
        /// </summary>
        public string Number
        {
            get => number;
            set 
            { 
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }
        /// <summary>
        /// Код подразделения паспорта.
        /// </summary>
        public string DivisionCode
        {
            get => divisionCode;
            set 
            { 
                divisionCode = value;
                OnPropertyChanged(nameof(DivisionCode));
            }
        }
        /// <summary>
        /// Указывает, есть ли у пользователя отчество.
        /// </summary>
        public bool DoesUserHavePatronymic
        {
            get => Patronymic != null;
            set 
            {
                Patronymic = value ? "" : null;
                OnPropertyChanged(nameof(DoesUserHavePatronymic));
            }
        }
        /// <summary>
        /// Указывает, сохраняются ли данные в текущий момент.
        /// </summary>
        public bool IsDataSave
        {
            get => isDataSave;
            set
            {
                isDataSave = value;
                OnPropertyChanged(nameof(IsDataSave));
            }
        }
        /// <summary>
        /// Указывает, сохранены ли данные.
        /// </summary>
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

        #region Methods
        /// <inheritdoc cref="ViewModelBase.Dispose"/>
        public override void Dispose() => base.Dispose();
        #endregion

        #region Commands
        /// <summary>
        /// Команда перехода на страницу аккаунта.
        /// </summary>
        public ICommand NavigateAccountCommand { get; }
        /// <summary>
        /// Команда сохранения данных пользователя.
        /// </summary>
        public ICommand SaveCommand { get; }
        #endregion
    }
}
