using EWallet.Stores;
using EWallet.ViewModels;
using EWallet.Services;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EWallet
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует App.cs.
        /// </summary>
        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<UserStore>();
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ModalNavigationStore>();

            services.AddSingleton(s => CreateHomeNavigationService(s));
            services.AddSingleton<CloseModalNavigationService>();

            services.AddTransient(s 
                => new HomeViewModel(CreateAuthorizationNavigationService(serviceProvider), 
                    CreateRegistrationNavigationService(serviceProvider)));
            services.AddTransient(CreateAuthorizationViewModel);
            services.AddTransient(CreateRegistrationViewModel);
            services.AddTransient(s
                => new AccountViewModel(s.GetRequiredService<UserStore>(),
                    CreateHomeNavigationService(s), CreateUserProfileNavigationService(s)));
            services.AddTransient(s => new UserProfileViewModel(s.GetRequiredService<UserStore>(),
                    CreateAccountNavigationService(s), CreateAuthorizationNavigationService(s)));
            services.AddSingleton(CreateNavigationBarViewModel);
            services.AddSingleton<MainViewModel>();

            services.AddSingleton(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            serviceProvider = services.BuildServiceProvider();
        }
        #endregion

        #region EventHandlers
        /// <inheritdoc cref="StartupEventHandler"/>
        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService initialNavigationService = serviceProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();

            MainWindow = serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }
        #endregion

        #region Methods
        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider) 
            => new NavigationBarViewModel(
                        serviceProvider.GetRequiredService<UserStore>(),
                        CreateHomeNavigationService(serviceProvider),
                        CreateAuthorizationNavigationService(serviceProvider),
                        CreateRegistrationNavigationService(serviceProvider),
                        CreateAccountNavigationService(serviceProvider),
                        CreateUserProfileNavigationService(serviceProvider));

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к HomeViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel домашней страницы.</returns>
        public INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider) 
            => new LayoutNavigationService<HomeViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), 
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<HomeViewModel>());

        public AuthorizationViewModel CreateAuthorizationViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            CompositeNavigationService navigationService = new CompositeNavigationService(
                closeModalNavigationService,
                CreateAccountNavigationService(serviceProvider));

            return new AuthorizationViewModel(serviceProvider.GetRequiredService<UserStore>(),
                navigationService,
                CreateRegistrationNavigationService(serviceProvider),
                closeModalNavigationService);
        }

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к AuthorizationViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel страницы авторизации.</returns>
        public INavigationService CreateAuthorizationNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
            return new ModalNavigationService<AuthorizationViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<AuthorizationViewModel>());
        }

        public RegistrationViewModel CreateRegistrationViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();

            return new RegistrationViewModel(CreateAccountNavigationService(serviceProvider),
                closeModalNavigationService, serviceProvider.GetRequiredService<UserStore>());
        }

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к RegistrationViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel страницы регистрации.</returns>
        public INavigationService CreateRegistrationNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

            return new ModalNavigationService<RegistrationViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<RegistrationViewModel>());
        }

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к AccountViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel аккаунта пользователя.</returns>
        public INavigationService CreateAccountNavigationService(IServiceProvider serviceProvider) 
            => new LayoutNavigationService<AccountViewModel>(serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<AccountViewModel>());

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к UserProfileViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel профиля пользователя.</returns>
        public INavigationService CreateUserProfileNavigationService(IServiceProvider serviceProvider)
            => new LayoutNavigationService<UserProfileViewModel>(serviceProvider.GetRequiredService<NavigationStore>(), 
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<UserProfileViewModel>());
        #endregion
    }
}
