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
        private bool isRegistrationButtonEnabled;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует объект класса RegistrationViewModel.
        /// </summary>
        /// <param name="navigationStore">Навигационное хранилище.</param>
        public RegistrationViewModel(INavigationService authorizationNavigationService, INavigationService homeNavigationService)
        {

            NavigateCommand = new NavigateCommand(authorizationNavigationService);
            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            RegisterUserCommand = new RegisterUserCommand(this, authorizationNavigationService);
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
