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
            // ServiceCollection для корректной генерации классов
            // во время исполнения программы
            IServiceCollection services = new ServiceCollection();

            // Добавление хранилищ в единственном
            // экземпляре в коллекцию сервисов
            services.AddSingleton<UserStore>();
            services.AddSingleton<CardStore>();
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ModalNavigationStore>();

            // Добавление навигационных сервисов в
            // единственном экземпляре в коллекцию
            // сервисов
            services.AddSingleton(CreateHomeNavigationService);
            services.AddSingleton<CloseModalNavigationService>();

            // Добавление основной ViewModel в
            // единственном экземпляре в коллекцию
            // сервисов
            services.AddSingleton<MainViewModel>();

            // Добавление сменяющих друг друга
            // ViewModel-ей в коллекцию сервисов
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

            // Добавление в коллекцию сервисов
            // единственного экземпляра MainWindow
            services.AddSingleton(s => new MainWindow()
            {
                // Задание уже добавленной в коллекцию
                // сервисов MainViewModel в качестве
                // DataContext для MainWindow
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            // Построение IServiceProvider
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
        /// <summary>
        /// Создает экземпляр <see cref="NavigationBarViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="NavigationBarViewModel"/>.</returns>
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
        /// <summary>
        /// Создает экземпляр <see cref="HomeViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="HomeViewModel"/>.</returns>
        public HomeViewModel CreateHomeViewModel(IServiceProvider serviceProvider)
            => new HomeViewModel(CreateAuthorizationNavigationService(serviceProvider),
                    CreateRegistrationNavigationService(serviceProvider));
        /// <summary>
        /// Создает экземпляр <see cref="INavigationService"/> 
        /// с переходом на <see cref="HomeViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="INavigationService"/>
        /// с привязкой к <see cref="HomeViewModel"/></returns>
        public INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
            => new LayoutNavigationService<HomeViewModel>(serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<HomeViewModel>());
        #endregion

        #region Authorization
        /// <summary>
        /// Создает экземпляр <see cref="AuthorizationViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="AuthorizationViewModel"/>.</returns>
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
        /// Создает экземпляр <see cref="INavigationService"/> 
        /// с переходом на <see cref="AuthorizationViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="INavigationService"/>
        /// с привязкой к <see cref="AuthorizationViewModel"/></returns>
        public INavigationService CreateAuthorizationNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
            return new ModalNavigationService<AuthorizationViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<AuthorizationViewModel>());
        }
        #endregion

        #region Registration
        /// <summary>
        /// Создает экземпляр <see cref="RegistrationViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="RegistrationViewModel"/>.</returns>
        public RegistrationViewModel CreateRegistrationViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(
                closeModalNavigationService, CreateAccountNavigationService(serviceProvider));

            return new RegistrationViewModel(serviceProvider.GetRequiredService<UserStore>(),
                CreateAuthorizationNavigationService(serviceProvider), accountNavigationService, closeModalNavigationService);
        }
        /// <summary>
        /// Создает экземпляр <see cref="INavigationService"/> 
        /// с переходом на <see cref="RegistrationViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="INavigationService"/>
        /// с привязкой к <see cref="RegistrationViewModel"/></returns>
        public INavigationService CreateRegistrationNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

            return new ModalNavigationService<RegistrationViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<RegistrationViewModel>());
        }
        #endregion

        #region Account
        /// <summary>
        /// Создает экземпляр <see cref="AccountViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="AccountViewModel"/>.</returns>
        public AccountViewModel CreateAccountViewModel(IServiceProvider serviceProvider)
            => new AccountViewModel(serviceProvider.GetRequiredService<UserStore>(),
                    CreateHomeNavigationService(serviceProvider), CreateUserProfileNavigationService(serviceProvider),
                    CreateTransferNavigationService(serviceProvider), CreateWithdrawNavigationService(serviceProvider),
                    CreateRefillNavigationService(serviceProvider), CreateCardManagmentNavigationService(serviceProvider),
                    CreateExpenseReportNavigationService(serviceProvider));
        /// <summary>
        /// Создает экземпляр <see cref="INavigationService"/> 
        /// с переходом на <see cref="AccountViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="INavigationService"/>
        /// с привязкой к <see cref="AccountViewModel"/></returns>
        public INavigationService CreateAccountNavigationService(IServiceProvider serviceProvider)
            => new LayoutNavigationService<AccountViewModel>(serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>(),
                () => serviceProvider.GetRequiredService<AccountViewModel>());
        #endregion

        #region UserProfile
        /// <summary>
        /// Создает экземпляр <see cref="UserProfileViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="UserProfileViewModel"/>.</returns>
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
        /// Создает экземпляр <see cref="INavigationService"/> 
        /// с переходом на <see cref="UserProfileViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="INavigationService"/>
        /// с привязкой к <see cref="UserProfileViewModel"/></returns>
        public INavigationService CreateUserProfileNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
            return new ModalNavigationService<UserProfileViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<UserProfileViewModel>());
        }
        #endregion

        #region Transfer
        /// <summary>
        /// Создает экземпляр <see cref="TransferViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="TransferViewModel"/>.</returns>
        public TransferViewModel CreateTransferViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(
                closeModalNavigationService,
                CreateAccountNavigationService(serviceProvider));

            return new TransferViewModel(serviceProvider.GetRequiredService<UserStore>(),
                accountNavigationService);
        }
        /// <summary>
        /// Создает экземпляр <see cref="INavigationService"/> 
        /// с переходом на <see cref="TransferViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="INavigationService"/>
        /// с привязкой к <see cref="TransferViewModel"/></returns>
        public INavigationService CreateTransferNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
            return new ModalNavigationService<TransferViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<TransferViewModel>());
        }
        #endregion

        #region Withdraw
        /// <summary>
        /// Создает экземпляр <see cref="WithdrawViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="WithdrawViewModel"/>.</returns>
        private WithdrawViewModel CreateWithdrawViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(
                closeModalNavigationService,
                CreateAccountNavigationService(serviceProvider));

            return new WithdrawViewModel(serviceProvider.GetRequiredService<UserStore>(),
                accountNavigationService);
        }
        /// <summary>
        /// Создает экземпляр <see cref="INavigationService"/> 
        /// с переходом на <see cref="WithdrawViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="INavigationService"/>
        /// с привязкой к <see cref="WithdrawViewModel"/></returns>
        public INavigationService CreateWithdrawNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
            return new ModalNavigationService<WithdrawViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<WithdrawViewModel>());
        }
        #endregion

        #region Refill
        /// <summary>
        /// Создает экземпляр <see cref="RefillViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="RefillViewModel"/>.</returns>
        private RefillViewModel CreateRefillViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(
                closeModalNavigationService,
                CreateAccountNavigationService(serviceProvider));

            return new RefillViewModel(serviceProvider.GetRequiredService<UserStore>(),
                accountNavigationService);
        }
        /// <summary>
        /// Создает экземпляр <see cref="INavigationService"/> 
        /// с переходом на <see cref="RefillViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="INavigationService"/>
        /// с привязкой к <see cref="RefillViewModel"/></returns>
        public INavigationService CreateRefillNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
            return new ModalNavigationService<RefillViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<RefillViewModel>());
        }
        #endregion

        #region ExpenseReport
        /// <summary>
        /// Создает экземпляр <see cref="ExpenseReportViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="ExpenseReportViewModel"/>.</returns>
        private ExpenseReportViewModel CreateExpenseReportViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(closeModalNavigationService, 
                CreateAccountNavigationService(serviceProvider));

            return new ExpenseReportViewModel(serviceProvider.GetRequiredService<UserStore>(),
                accountNavigationService);
        }
        /// <summary>
        /// Создает экземпляр <see cref="INavigationService"/> 
        /// с переходом на <see cref="ExpenseReportViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="INavigationService"/>
        /// с привязкой к <see cref="ExpenseReportViewModel"/></returns>
        public INavigationService CreateExpenseReportNavigationService(IServiceProvider serviceProvider) 
            => new ModalNavigationService<ExpenseReportViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<ExpenseReportViewModel>());
        #endregion

        #region CardManagment
        /// <summary>
        /// Создает экземпляр <see cref="CardManagmentViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="CardManagmentViewModel"/>.</returns>
        private CardManagmentViewModel CreateCardManagmentViewModel(IServiceProvider serviceProvider)
        {
            var closeModalNavigationService = serviceProvider.GetRequiredService<CloseModalNavigationService>();
            var accountNavigationService = new CompositeNavigationService(closeModalNavigationService,
                CreateAccountNavigationService(serviceProvider));

            return new CardManagmentViewModel(serviceProvider.GetRequiredService<UserStore>(),
                serviceProvider.GetRequiredService<CardStore>(),
                accountNavigationService, CreateCardNavigationService(serviceProvider));
        }
        /// <summary>
        /// Создает экземпляр <see cref="INavigationService"/> 
        /// с переходом на <see cref="CardManagmentViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="INavigationService"/>
        /// с привязкой к <see cref="CardManagmentViewModel"/></returns>
        public INavigationService CreateCardManagmentNavigationService(IServiceProvider serviceProvider)
        {
            var modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

            return new ModalNavigationService<CardManagmentViewModel>(modalNavigationStore,
                () => serviceProvider.GetRequiredService<CardManagmentViewModel>());
        }
        #endregion

        #region Card
        /// <summary>
        /// Создает экземпляр <see cref="CardManagmentViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="CardManagmentViewModel"/>.</returns>
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
        /// <summary>
        /// Создает экземпляр <see cref="INavigationService"/> 
        /// с переходом на <see cref="CardManagmentViewModel"/>.
        /// </summary>
        /// <param name="serviceProvider">Позволяет корректно 
        /// извлекать объекты службы (классы) во время работы
        /// приложения.</param>
        /// <returns>Настроенный экземпляр <see cref="INavigationService"/>
        /// с привязкой к <see cref="CardManagmentViewModel"/></returns>
        public INavigationService CreateCardNavigationService(IServiceProvider serviceProvider) 
            => new ModalNavigationService<CardViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<CardViewModel>());
        #endregion
        #endregion
    }
}
