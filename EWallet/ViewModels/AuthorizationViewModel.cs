using EWallet.Commands;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel страницы авторизации.
    /// </summary>
    public class AuthorizationViewModel : ViewModelBase
    {
        #region Fields
        private string login = "";
        private string password = "";
        private bool isEnterButtonEnabled = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует объект класса AuthorizationViewModel.
        /// </summary>
        /// <param name="navigationBarViewModel">ViewModel страницы регистрации.</param>
        /// <param name="navigationStore">Навигационное хранилище.</param>
        public AuthorizationViewModel(UserStore userStore, 
            INavigationService accountNavigationService, 
            INavigationService registrationNavigationService, 
            INavigationService homeNavigationService)
        {

            NavigateCommand = new NavigateCommand(registrationNavigationService);
            AuthorizeUserCommand = new AuthorizeUserCommand(this, accountNavigationService, userStore);
            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
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
                IsEnterButtonEnabled = EnableEnterButton();
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
                IsEnterButtonEnabled = EnableEnterButton();
            }
        }

        /// <summary>
        /// Флаг, отвечающий за включение и отключение кнопки входа.
        /// </summary>
        /// <value>
        /// Булева переменная, содержащая состояние кнопки входа.
        /// </value>
        public bool IsEnterButtonEnabled
        {
            get => isEnterButtonEnabled;
            set
            {
                isEnterButtonEnabled = value;
                OnPropertyChanged(nameof(IsEnterButtonEnabled));
            }
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand { get; }
        public ICommand AuthorizeUserCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand ExitAppCommand { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Метод, получающий значение, ответственное 
        /// за включение кнопки входа.
        /// </summary>
        /// <returns>Булево значение, показывающее, необходимо ли 
        /// активировать кнопку входа.</returns>
        private bool EnableEnterButton()
            => login != "" && password != "";

        public override void Dispose() => base.Dispose();
        #endregion
    }
}
