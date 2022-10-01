using EWallet.Commands;
using EWallet.Stores;
using EWallet.Services;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel страницы регистрации пользователей.
    /// </summary>
    public class RegistrationViewModel : ViewModelBase
    {
        #region Fields
        private string login = "";
        private string password = "";
        private string repeatedPassword = "";
        private string firstName;
        private string lastName;
        private string patronymic;

        private bool doesUserHavePatronymic;
        private bool isRegistrationButtonEnabled;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует объект класса RegistrationViewModel.
        /// </summary>
        /// <param name="navigationStore">Навигационное хранилище.</param>
        public RegistrationViewModel(INavigationService authorizationNavigationService, 
            INavigationService accountNavigationService, 
            INavigationService homeNavigationService, UserStore userStore)
        {
            NavigateCommand = new NavigateCommand(authorizationNavigationService);
            NavigateAccountCommand = new NavigateCommand(accountNavigationService);
            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            RegisterUserCommand = new RegisterUserCommand(this, accountNavigationService, userStore);
            ExitAppCommand = new ExitAppCommand();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        /// <value>
        /// Строковое представление логина пользователя.
        /// </value>
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
                IsRegistrationButtonEnabled = EnableRegistrationButton();
            }
        }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        /// <value>
        /// Строковое представление пароля пользователя.
        /// </value>
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
                IsRegistrationButtonEnabled = EnableRegistrationButton();
            }
        }

        /// <summary>
        /// Повторно введённый пароль пользователя.
        /// </summary>
        /// <value>
        /// Строковое представление повторного пароля пользователя.
        /// </value>
        public string RepeatedPassword
        {
            get => repeatedPassword;
            set
            {
                repeatedPassword = value;
                OnPropertyChanged(nameof(RepeatedPassword));
                IsRegistrationButtonEnabled = EnableRegistrationButton();
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

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public bool DoesUserHavePatronymic
        {
            get => doesUserHavePatronymic;
            set
            {
                doesUserHavePatronymic = value;
                OnPropertyChanged(nameof(DoesUserHavePatronymic));
            }
        }

        /// <summary>
        /// Флаг, отвечающий за включение и отключение кнопки регистрации.
        /// </summary>
        /// <value>
        /// Булева переменная, содержащая состояние кнопки регистрации.
        /// </value>
        public bool IsRegistrationButtonEnabled
        {
            get => isRegistrationButtonEnabled;
            set
            {
                isRegistrationButtonEnabled = value;
                OnPropertyChanged(nameof(IsRegistrationButtonEnabled));
            }
        }
        #endregion

        #region Commands
        public ICommand RegisterUserCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand ExitAppCommand { get; }
        #endregion

        #region Methods

        /// <summary>
        /// Метод, получающий значение, ответственное за включение 
        /// кнопки регистрации.
        /// </summary>
        /// <returns>Булево значение, показывающее, необходимо ли 
        /// активировать кнопку регистрации.</returns>
        private bool EnableRegistrationButton()
            => login != "" && password != "" &&
                repeatedPassword != "" && password == repeatedPassword;

        public override void Dispose() => base.Dispose();
        #endregion
    }
}
