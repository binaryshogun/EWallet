using EWallet.Commands;
using EWallet.Services;
using EWallet.Stores;
using System.Windows.Input;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel навигационной панели.
    /// </summary>
    public class NavigationBarViewModel : ViewModelBase
    {
        #region Fields
        private readonly UserStore userStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует объект класса NavigationBarViewModel.
        /// </summary>
        /// <param name="homeNavigationService">NavigationService для домашней страницы.</param>
        /// <param name="authorizationNavigationService">NavigationService для страницы авторизации.</param>
        /// <param name="registrationNavigationService">NavigationService для страницы регистрации.</param>
        /// <param name="accountNavigationService">NavigationService для аккаунта.</param>
        /// <param name="userProfileNavigationService">NavigationService для профиля пользователя.</param>
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

            this.userStore.CurrentUserChanged += OnCurrentAccountChanged;
        }
        #endregion

        #region Properties
        public bool IsLoggedOut => userStore.IsLoggedOut;
        public bool IsLoggedIn => userStore.IsLoggedIn;
        #endregion

        #region Commands
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateAuthorizationCommand { get; }
        public ICommand NavigateRegistrationCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand NavigateUserProfileCommand { get; }
        public ICommand ExitAppCommand { get; }
        #endregion

        #region Methods
        private void OnCurrentAccountChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsLoggedOut));
        }

        public override void Dispose() 
        {
            userStore.CurrentUserChanged -= OnCurrentAccountChanged;

            base.Dispose(); 
        }
        #endregion
    }
}
