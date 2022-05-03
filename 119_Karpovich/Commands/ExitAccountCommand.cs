using _119_Karpovich.Services;
using _119_Karpovich.Stores;
using _119_Karpovich.ViewModels;
using System.Windows;

namespace _119_Karpovich.Commands
{
    /// <summary>
    /// Команда выхода из учётной записи.
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel для перехода при выходе из системы.</typeparam>
    internal class ExitAccountCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly INavigationService navigationService;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует команду выхода из аккаунта.
        /// </summary>
        /// <param name="navigationService">Сервис навигации, привязанный к TViewModel.</param>
        public ExitAccountCommand(UserStore userStore, INavigationService navigationService)
        {
            this.userStore = userStore;
            this.navigationService = navigationService;
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter)
        {
            if (MessageBox.Show("Вы действительно хотите выйти из аккаунта?", "Предупреждение", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                userStore.Logout();
                navigationService.Navigate();
            }
        }
        #endregion
    }
}
