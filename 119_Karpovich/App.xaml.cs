using _119_Karpovich.Stores;
using _119_Karpovich.ViewModels;
using _119_Karpovich.Services;
using System.Windows;

namespace _119_Karpovich
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields
        private readonly NavigationStore navigationStore;
        private readonly NavigationBarViewModel navigationBarViewModel;
        private readonly UserStore userStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует App.cs.
        /// </summary>
        public App()
        {
            userStore = new UserStore();
            navigationStore = new NavigationStore();
            navigationBarViewModel = new NavigationBarViewModel(
                userStore,
                CreateHomeNavigationService(), 
                CreateAuthorizationNavigationService(),
                CreateRegistrationNavigationService(),
                CreateAccountNavigationService(),
                CreateUserProfileNavigationService());
        }
        #endregion

        #region EventHandlers
        /// <inheritdoc cref="StartupEventHandler"/>
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationService<HomeViewModel> homeNavigationService = CreateHomeNavigationService();
            homeNavigationService.Navigate();

            MainWindow = new MainWindow()
            {
                DataContext = new DisplayViewModel(navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Метод, создающий NavigationService, привязанный к HomeViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel домашней страницы.</returns>
        public NavigationService<HomeViewModel> CreateHomeNavigationService() 
            => new NavigationService<HomeViewModel>(navigationStore,
                () => new HomeViewModel(navigationBarViewModel));

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к AuthorizationViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel страницы авторизации.</returns>
        public NavigationService<AuthorizationViewModel> CreateAuthorizationNavigationService() 
            => new NavigationService<AuthorizationViewModel>(navigationStore, 
                () => new AuthorizationViewModel(userStore, navigationBarViewModel, 
                    CreateAccountNavigationService(), CreateRegistrationNavigationService()));

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к RegistrationViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel страницы регистрации.</returns>
        public NavigationService<RegistrationViewModel> CreateRegistrationNavigationService() 
            => new NavigationService<RegistrationViewModel>(navigationStore, 
                () => new RegistrationViewModel(navigationBarViewModel, CreateAuthorizationNavigationService()));

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к AccountViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel аккаунта пользователя.</returns>
        public NavigationService<AccountViewModel> CreateAccountNavigationService() 
            => new NavigationService<AccountViewModel>(navigationStore, 
                () => new AccountViewModel(userStore, navigationBarViewModel, CreateAuthorizationNavigationService()));

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к UserProfileViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel профиля пользователя.</returns>
        public NavigationService<UserProfileViewModel> CreateUserProfileNavigationService()
            => new NavigationService<UserProfileViewModel>(navigationStore,
                () => new UserProfileViewModel(userStore, navigationBarViewModel, CreateAccountNavigationService(), CreateAuthorizationNavigationService()));
        #endregion
    }
}
