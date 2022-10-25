using EWallet.Stores;
using EWallet.ViewModels;
using EWallet.Services;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using EWallet.Commands;

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
            services.AddSingleton<CardStore>();
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ModalNavigationStore>();

            services.AddSingleton(CreateHomeNavigationService);
            services.AddSingleton<CloseModalNavigationService>();

            services.AddSingleton<MainViewModel>();
            services.AddTransient(CreateNavigationBarViewModel);
            services.AddTransient(CreateHomeViewModel);
            services.AddTransient(CreateAuthorizationViewModel);
            services.AddTransient(CreateRegistrationViewModel);
            services.AddTransient(CreateAccountViewModel);
            services.AddTransient(CreateTransferViewModel);
            services.AddTransient(CreateWithdrawViewModel);
            services.AddTransient(CreateRefillViewModel);
            services.AddTransient(CreateUserProfileViewModel);
            services.AddTransient(CreateCardManagmentViewModel);
            services.AddTransient(CreateExpenseReportViewModel);
            services.AddTransient(CreateCardViewModel);

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

        #region NavigationBar
        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
            => new NavigationBarViewModel(
                        serviceProvider.GetRequiredService<UserStore>(),
                        CreateHomeNavigationService(serviceProvider),
                        CreateAuthorizationNavigationService(serviceProvider),
                        CreateRegistrationNavigationService(serviceProvider),
                        CreateAccountNavigationService(serviceProvider),
                        CreateUserProfileNavigationService(serviceProvider));
        #endregion

        #region Home
        public HomeViewModel CreateHomeViewModel(IServiceProvider serviceProvider)
            => new HomeViewModel(CreateAuthorizationNavigationService(serviceProvider),
                    CreateRegistrationNavigationService(serviceProvider));

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к HomeViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel домашней страницы.</returns>
        public INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
            => new LayoutNavigationService<HomeViewModel>(serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<HomeViewModel>());
        #endregion

        #region Authorization
        public AuthorizationViewModel CreateAuthorizationViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(
                closeModalNavigationService,
                CreateAccountNavigationService(serviceProvider));

            return new AuthorizationViewModel(serviceProvider.GetRequiredService<UserStore>(),
                accountNavigationService,
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
        #endregion

        #region Registration
        public RegistrationViewModel CreateRegistrationViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(
                closeModalNavigationService, CreateAccountNavigationService(serviceProvider));

            return new RegistrationViewModel(serviceProvider.GetRequiredService<UserStore>(),
                CreateAuthorizationNavigationService(serviceProvider), accountNavigationService, closeModalNavigationService);
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
        #endregion

        #region Account
        public AccountViewModel CreateAccountViewModel(IServiceProvider serviceProvider)
            => new AccountViewModel(serviceProvider.GetRequiredService<UserStore>(),
                    CreateHomeNavigationService(serviceProvider), CreateUserProfileNavigationService(serviceProvider),
                    CreateTransferNavigationService(serviceProvider), CreateWithdrawNavigationService(serviceProvider),
                    CreateRefillNavigationService(serviceProvider), CreateCardManagmentNavigationService(serviceProvider),
                    CreateExpenseReportNavigationService(serviceProvider));

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к AccountViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel аккаунта пользователя.</returns>
        public INavigationService CreateAccountNavigationService(IServiceProvider serviceProvider)
            => new LayoutNavigationService<AccountViewModel>(serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<AccountViewModel>());
        #endregion

        #region UserProfile
        public UserProfileViewModel CreateUserProfileViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(
                closeModalNavigationService,
                CreateAccountNavigationService(serviceProvider));

            return new UserProfileViewModel(serviceProvider.GetRequiredService<UserStore>(),
                accountNavigationService);
        }

        /// <summary>
        /// Метод, создающий NavigationService, привязанный к UserProfileViewModel.
        /// </summary>
        /// <returns>Навигационный сервис, привязанный к ViewModel профиля пользователя.</returns>
        public INavigationService CreateUserProfileNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
            return new ModalNavigationService<UserProfileViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<UserProfileViewModel>());
        }
        #endregion

        #region Transfer
        public TransferViewModel CreateTransferViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(
                closeModalNavigationService,
                CreateAccountNavigationService(serviceProvider));

            return new TransferViewModel(serviceProvider.GetRequiredService<UserStore>(),
                accountNavigationService);
        }

        public INavigationService CreateTransferNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
            return new ModalNavigationService<TransferViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<TransferViewModel>());
        }
        #endregion

        #region Withdraw
        private WithdrawViewModel CreateWithdrawViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(
                closeModalNavigationService,
                CreateAccountNavigationService(serviceProvider));

            return new WithdrawViewModel(serviceProvider.GetRequiredService<UserStore>(),
                accountNavigationService);
        }
        public INavigationService CreateWithdrawNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
            return new ModalNavigationService<WithdrawViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<WithdrawViewModel>());
        }
        #endregion

        #region Refill
        private RefillViewModel CreateRefillViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(
                closeModalNavigationService,
                CreateAccountNavigationService(serviceProvider));

            return new RefillViewModel(serviceProvider.GetRequiredService<UserStore>(),
                accountNavigationService);
        }

        public INavigationService CreateRefillNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
            return new ModalNavigationService<RefillViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<RefillViewModel>());
        }
        #endregion

        #region ExpenseReport
        private ExpenseReportViewModel CreateExpenseReportViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(closeModalNavigationService, 
                CreateAccountNavigationService(serviceProvider));

            return new ExpenseReportViewModel(serviceProvider.GetRequiredService<UserStore>(),
                accountNavigationService);
        }

        public INavigationService CreateExpenseReportNavigationService(IServiceProvider serviceProvider) 
            => new ModalNavigationService<ExpenseReportViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<ExpenseReportViewModel>());
        #endregion

        #region CardManagment
        private CardManagmentViewModel CreateCardManagmentViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(closeModalNavigationService,
                CreateAccountNavigationService(serviceProvider));

            return new CardManagmentViewModel(serviceProvider.GetRequiredService<UserStore>(),
                serviceProvider.GetRequiredService<CardStore>(),
                accountNavigationService, CreateCardNavigationService(serviceProvider));
        }

        public INavigationService CreateCardManagmentNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

            return new ModalNavigationService<CardManagmentViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<CardManagmentViewModel>());
        }
        #endregion

        #region Card
        private CardViewModel CreateCardViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(closeModalNavigationService,
                CreateAccountNavigationService(serviceProvider));

            return new CardViewModel(serviceProvider.GetRequiredService<UserStore>(),
                serviceProvider.GetRequiredService<CardStore>(),
                CreateCardManagmentNavigationService(serviceProvider),
                accountNavigationService);
        }

        public INavigationService CreateCardNavigationService(IServiceProvider serviceProvider) 
            => new ModalNavigationService<CardViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<CardViewModel>());
        #endregion

        #endregion
    }
}
