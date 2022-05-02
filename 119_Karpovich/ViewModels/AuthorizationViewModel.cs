using _119_Karpovich.Commands;
using _119_Karpovich.Models;
using _119_Karpovich.Services;
using _119_Karpovich.Stores;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace _119_Karpovich.ViewModels
{
    /// <summary>
    /// ViewModel страницы авторизации.
    /// </summary>
    public class AuthorizationViewModel : ViewModelBase
    {
        #region Fields
        private string login = "";
        private string password = "";
        private string timeNow;
        private bool isEnterButtonEnabled = false;
        private readonly DispatcherTimer updateTimer;
        #endregion

        #region Constructors
        public AuthorizationViewModel(NavigationStore navigationStore)
        {
            ParameterNavigationService<User, AccountViewModel> navigationService = new ParameterNavigationService<User, AccountViewModel>(
                navigationStore, (parameter) => new AccountViewModel(parameter, navigationStore));

            NavigateCommand = new NavigateCommand<RegistrationViewModel>(new NavigationService<RegistrationViewModel>(
                navigationStore, () => new RegistrationViewModel(navigationStore)));
            AuthorizeUserCommand = new AuthorizeUserCommand(this, navigationService);
            ExitAppCommand = new ExitAppCommand();

            timeNow = DateTime.Now.ToString("g");

            updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            updateTimer.Tick += new EventHandler(UpdateTime);
            updateTimer.Start();
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
        /// Текущее локальное время.
        /// </summary>
        /// <value>
        /// Строка, содержащее текущее время.
        /// </value>
        public string TimeNow
        {
            get => timeNow;
            set
            {
                timeNow = value;
                OnPropertyChanged(nameof(TimeNow));
            }
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand { get; }
        public ICommand AuthorizeUserCommand { get; }
        public ICommand ExitAppCommand { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Метод, получающий значение, ответственное 
        /// за включение кнопки входа.
        /// </summary>
        /// <returns>Булево значение, показывающее, необходимо ли 
        /// активировать кнопку входа.</returns>
        private bool EnableEnterButton()
            => login != "" && password != "";

        /// <summary>
        /// Обработчик события обновления времени в таймере.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Данные события.</param>
        private void UpdateTime(object sender, EventArgs e)
            => TimeNow = DateTime.Now.ToString("g");
        #endregion
    }
}
