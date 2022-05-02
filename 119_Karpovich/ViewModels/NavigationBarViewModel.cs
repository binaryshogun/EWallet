using _119_Karpovich.Commands;
using _119_Karpovich.Services;
using _119_Karpovich.Stores;
using System.Windows.Input;

namespace _119_Karpovich.ViewModels
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
        public NavigationBarViewModel(UserStore userStore, NavigationService<HomeViewModel> homeNavigationService, 
            NavigationService<AuthorizationViewModel> authorizationNavigationService, 
            NavigationService<RegistrationViewModel> registrationNavigationService, 
            NavigationService<AccountViewModel> accountNavigationService, 
            NavigationService<UserProfileViewModel> userProfileNavigationService)
        {
            this.userStore = userStore;
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(homeNavigationService);
            NavigateAuthorizationCommand = new NavigateCommand<AuthorizationViewModel>(authorizationNavigationService);
            NavigateRegistrationCommand = new NavigateCommand<RegistrationViewModel>(registrationNavigationService);
            NavigateAccountCommand = new NavigateCommand<AccountViewModel>(accountNavigationService);
            NavigateUserProfileCommand = new NavigateCommand<UserProfileViewModel>(userProfileNavigationService);
            ExitAppCommand = new ExitAppCommand();
        }
        #endregion

        #region Properties
        public bool IsLoggedOut => userStore.CurrentUser == null ? true : false;
        public bool IsLoggedIn => userStore.CurrentUser == null ? false : true;
        #endregion

        #region Commands
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateAuthorizationCommand { get; }
        public ICommand NavigateRegistrationCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand NavigateUserProfileCommand { get; }
        public ICommand ExitAppCommand { get; }
        #endregion
    }
}
