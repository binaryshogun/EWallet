using EWallet.Commands;
using EWallet.Stores;
using EWallet.Services;
using System.Windows.Input;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel страницы регистрации пользователей.
    /// </summary>
    public sealed class RegistrationViewModel : ViewModelBase
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
        private bool isUserAuthorizing;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RegistrationViewModel"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>,
        /// содержащий данные о текущем пользователе.</param>
        /// <param name="authorizationNavigationService">
        /// <see cref="INavigationService"/>, совершающий
        /// переход на <see cref="AuthorizationViewModel"/>.</param>
        /// <param name="accountNavigationService">
        /// <see cref="INavigationService"/>, совершающий
        /// переход на <see cref="AccountViewModel"/>.</param>
        /// <param name="homeNavigationService">
        /// <see cref="INavigationService"/>, совершающий
        /// переход на <see cref="HomeViewModel"/>.</param>
        public RegistrationViewModel(UserStore userStore, INavigationService authorizationNavigationService, 
            INavigationService accountNavigationService, 
            INavigationService homeNavigationService)
        {
            NavigateCommand = new NavigateCommand(authorizationNavigationService);
            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            RegisterUserCommand = new RegisterUserCommand(this, accountNavigationService, userStore);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Логин пользователя.
        /// </summary>
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
        /// Указывает, есть ли у пользователя отчество.
        /// </summary>
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
        /// Указывает, доступна ли кнопка регистрации.
        /// </summary>
        public bool IsRegistrationButtonEnabled
        {
            get => isRegistrationButtonEnabled;
            set
            {
                isRegistrationButtonEnabled = value;
                OnPropertyChanged(nameof(IsRegistrationButtonEnabled));
            }
        }
        /// <summary>
        /// Указывает, проходит ли пользователь процедуру регистрации.
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
        /// Команда регистрации пользователя.
        /// </summary>
        public ICommand RegisterUserCommand { get; }
        /// <summary>
        /// Комманда перехода на страницу авторизации.
        /// </summary>
        public ICommand NavigateCommand { get; }
        /// <summary>
        /// Команда перехода на домашнюю страницу.
        /// </summary>
        public ICommand NavigateHomeCommand { get; }
        #endregion

        #region Methods

        /// <summary>
        /// Обновляет доступность кнопки регистрации.
        /// </summary>
        /// <returns><see langword="true"/> при заполненных полях ввода; 
        /// в обратном случае - <see langword="false"/>.</returns>
        private bool EnableRegistrationButton()
            => login != "" && password != "" &&
                repeatedPassword != "" && password == repeatedPassword;
        /// <inheritdoc cref="ViewModelBase.Dispose"/>
        public override void Dispose() 
            => base.Dispose();
        #endregion
    }
}
