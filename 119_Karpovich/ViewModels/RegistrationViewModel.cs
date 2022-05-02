using _119_Karpovich.Commands;
using _119_Karpovich.Stores;
using _119_Karpovich.Services;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace _119_Karpovich.ViewModels
{
    /// <summary>
    /// ViewModel страницы регистрации пользователей.
    /// </summary>
    public class RegistrationViewModel : ViewModelBase
    {
        #region Fields
        private string login = "";
        private string password = "";
        private string repeatedPassword = "";
        private string timeNow; 
        private bool isRegistrationButtonEnabled;
        private readonly DispatcherTimer updateTimer;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует объект класса RegistrationViewModel.
        /// </summary>
        /// <param name="navigationStore">Навигационное хранилище.</param>
        public RegistrationViewModel(NavigationStore navigationStore)
        {
            NavigateCommand = new NavigateCommand<AuthorizationViewModel>(new NavigationService<AuthorizationViewModel>(
                navigationStore, () => new AuthorizationViewModel(navigationStore)));
            RegisterUserCommand = new RegisterUserCommand(this, new NavigationService<AuthorizationViewModel>(
                navigationStore, () => new AuthorizationViewModel(navigationStore)));
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
                IsRegistrationButtonEnabled = EnableRegistrationButton();
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
                IsRegistrationButtonEnabled = EnableRegistrationButton();
            }
        }

        /// <summary>
        /// Повторно введённый пароль пользователя.
        /// </summary>
        /// <value>
        /// Строковое представление повторного пароля пользователя.
        /// </value>
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
        /// Флаг, отвечающий за включение и отключение кнопки регистрации.
        /// </summary>
        /// <value>
        /// Булева переменная, содержащая состояние кнопки регистрации.
        /// </value>
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
        public ICommand RegisterUserCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand ExitAppCommand { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Обработчик события обновления времени в таймере.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Данные события.</param>
        private void UpdateTime(object sender, EventArgs e)
            => TimeNow = DateTime.Now.ToString("g");

        /// <summary>
        /// Метод, получающий значение, ответственное за включение 
        /// кнопки регистрации.
        /// </summary>
        /// <returns>Булево значение, показывающее, необходимо ли 
        /// активировать кнопку регистрации.</returns>
        private bool EnableRegistrationButton()
            => login != "" && password != "" &&
                repeatedPassword != "" && password == repeatedPassword;
        #endregion
    }
}
