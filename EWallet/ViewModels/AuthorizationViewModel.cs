using EWallet.Commands;
using EWallet.Services;
using EWallet.Stores;
using System.Windows.Input;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel страницы авторизации.
    /// </summary>
    public sealed class AuthorizationViewModel : ViewModelBase
    {
        #region Fields
        private string login = "";
        private string password = "";
        private bool isEnterButtonEnabled = false;
        private bool isUserAuthorizing = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthorizationViewModel"/>.
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
        /// <summary>
        /// Указывает, находится ли пользователь в процессе авторизации.
        /// </summary>
        public bool IsUserAuthorizing
        {
            get => isUserAuthorizing;
            set
            {
                isUserAuthorizing = value;
                OnPropertyChanged(nameof(IsUserAuthorizing));
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Команда перехода на страницу регистрации.
        /// </summary>
        public ICommand NavigateCommand { get; }
        /// <summary>
        /// Команда авторизации пользователя.
        /// </summary>
        public ICommand AuthorizeUserCommand { get; }
        /// <summary>
        /// Команда перехода на начальную страницу.
        /// </summary>
        public ICommand NavigateHomeCommand { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Получает значение, указывающее, включена ли кнопка входа.
        /// </summary>
        /// <returns><see langword="true"/> при заполненных свойствах 
        /// <see cref="Login"/> и <see cref="Password"/>; в обратном случае - <see langword="false"/>.</returns>
        private bool EnableEnterButton()
            => Login != "" && Password != "";

        /// <inheritdoc cref="ViewModelBase.Dispose"/>
        public override void Dispose() 
            => base.Dispose();
        #endregion
    }
}
