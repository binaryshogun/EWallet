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
        private readonly ModalNavigationStore modalNavigationStore;
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
            modalNavigationStore = new ModalNavigationStore();
        }
        #endregion

        #region EventHandlers
        /// <inheritdoc cref="StartupEventHandler"/>
        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService homeNavigationService = CreateHomeNavigationService();
            homeNavigationService.Navigate();

            MainWindow = new MainWindow()
            {
                DataContext = new DisplayViewModel(navigationStore, modalNavigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
        #endregion

        #region Methods
        private NavigationBarViewModel CreateNavigationBarViewModel() => new NavigationBarViewModel(
                        userStore,
                        CreateHomeNavigationService(),
                        CreateAuthorizationNavigationService(),
                        CreateRegistrationNavigationService(),
                        CreateAccountNavigationService(),
                        CreateUserProfileNavigationService());

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к HomeViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel домашней страницы.</returns>
        public INavigationService CreateHomeNavigationService() 
            => new LayoutNavigationService<HomeViewModel>(navigationStore, CreateNavigationBarViewModel,
                () => new HomeViewModel(CreateAuthorizationNavigationService(), 
                    CreateRegistrationNavigationService()));

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к AuthorizationViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel страницы авторизации.</returns>
        public INavigationService CreateAuthorizationNavigationService()
        {
            CompositeNavigationService navigationService = new CompositeNavigationService(
                new CloseModalNavigationService(modalNavigationStore),
                CreateAccountNavigationService());

            CompositeNavigationService closeModalNavigationService = new CompositeNavigationService(
                new CloseModalNavigationService(modalNavigationStore),
                CreateHomeNavigationService());

            return new ModalNavigationService<AuthorizationViewModel>(modalNavigationStore,
                () => new AuthorizationViewModel(userStore,
                    navigationService, CreateRegistrationNavigationService(),
                    closeModalNavigationService));
        }

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к RegistrationViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel страницы регистрации.</returns>
        public INavigationService CreateRegistrationNavigationService()
        {
            CompositeNavigationService closeModalNavigationService = new CompositeNavigationService(
                new CloseModalNavigationService(modalNavigationStore),
                CreateHomeNavigationService());

            return new ModalNavigationService<RegistrationViewModel>(modalNavigationStore,
                () => new RegistrationViewModel(CreateAuthorizationNavigationService(),
                    closeModalNavigationService));
        }

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к AccountViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel аккаунта пользователя.</returns>
        public INavigationService CreateAccountNavigationService() 
            => new LayoutNavigationService<AccountViewModel>(navigationStore, CreateNavigationBarViewModel,
                () => new AccountViewModel(userStore, CreateHomeNavigationService()));

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к UserProfileViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel профиля пользователя.</returns>
        public INavigationService CreateUserProfileNavigationService()
            => new LayoutNavigationService<UserProfileViewModel>(navigationStore, CreateNavigationBarViewModel,
                () => new UserProfileViewModel(userStore, CreateAccountNavigationService(), 
                    CreateHomeNavigationService()));
        #endregion
    }
}
