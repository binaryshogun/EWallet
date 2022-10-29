using EWallet.Commands;
using EWallet.Services;
using EWallet.Stores;
using System.Windows.Input;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel навигационной панели.
    /// </summary>
    public sealed class NavigationBarViewModel : ViewModelBase
    {
        #region Fields
        private readonly UserStore userStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр 
        /// класса <see cref="NavigationBarViewModel"/>.
        /// </summary>
        /// <param name="homeNavigationService">
        /// <see cref="INavigationService"/>, совершающий 
        /// переход на <see cref="HomeViewModel"/>.</param>
        /// <param name="authorizationNavigationService">
        /// <see cref="INavigationService"/> совершающий,
        /// переход на <see cref="AuthorizationViewModel"/>.</param>
        /// <param name="registrationNavigationService">
        /// <see cref="INavigationService"/>, совершающий 
        /// переход на <see cref="RegistrationViewModel"/>.</param>
        /// <param name="accountNavigationService">
        /// <see cref="INavigationService"/>, совершающий
        /// переход на <see cref="AccountViewModel"/>.</param>
        /// <param name="userProfileNavigationService">
        /// <see cref="INavigationService"/>, совершающий
        /// переход на <see cref="UserProfileViewModel"/>.</param>
        public NavigationBarViewModel(UserStore userStore, INavigationService homeNavigationService, 
            INavigationService authorizationNavigationService,
            INavigationService registrationNavigationService,
            INavigationService accountNavigationService,
            INavigationService userProfileNavigationService)
        {
            this.userStore = userStore;
                        
            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            NavigateAccountCommand = new NavigateCommand(accountNavigationService);
            NavigateAuthorizationCommand = new NavigateCommand(authorizationNavigationService);
            NavigateRegistrationCommand = new NavigateCommand(registrationNavigationService);
            NavigateUserProfileCommand = new NavigateCommand(userProfileNavigationService);
            ExitAppCommand = new ExitAppCommand();

            this.userStore.CurrentUserChanged += OnCurrentUserChanged;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Указывает, что пользователь не авторизован.
        /// </summary>
        public bool IsLoggedOut 
            => userStore.IsLoggedOut;
        /// <summary>
        /// Указывает, что пользователь авторизован.
        /// </summary>
        public bool IsLoggedIn 
            => userStore.IsLoggedIn;
        #endregion

        #region Commands
        /// <summary>
        /// Команда перехода на начальную страницу.
        /// </summary>
        public ICommand NavigateHomeCommand { get; }
        /// <summary>
        /// Команда перехода на страницу авторизации.
        /// </summary>
        public ICommand NavigateAuthorizationCommand { get; }
        /// <summary>
        /// Команда перехода на страницу регистрации.
        /// </summary>
        public ICommand NavigateRegistrationCommand { get; }
        /// <summary>
        /// Команда перехода на страницу аккаунта.
        /// </summary>
        public ICommand NavigateAccountCommand { get; }
        /// <summary>
        /// Команда перехода на страницу профиля пользователя.
        /// </summary>
        public ICommand NavigateUserProfileCommand { get; }
        /// <summary>
        /// Команда выхода из приложения.
        /// </summary>
        public ICommand ExitAppCommand { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Обновляет поля <see cref="IsLoggedIn"/> 
        /// и <see cref="IsLoggedOut"/> при смене пользователя.
        /// </summary>
        public void OnCurrentUserChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsLoggedOut));
        }
        /// <summary>
        /// Освобождает ресурсы <see cref="NavigationBarViewModel"/>.
        /// </summary>
        public override void Dispose()
        {
            userStore.CurrentUserChanged -= OnCurrentUserChanged;

            base.Dispose();
        }
        #endregion
    }
}
