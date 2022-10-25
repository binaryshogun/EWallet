using EWallet.Services;
using EWallet.Stores;
using System.Windows;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда выхода из учётной записи.
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel для перехода при выходе из системы.</typeparam>
    public sealed class ExitAccountCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly INavigationService homeNavigationService;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует команду выхода из аккаунта.
        /// </summary>
        /// <param name="homeNavigationService">Сервис навигации, привязанный к HomeViewModel.</param>
        public ExitAccountCommand(UserStore userStore, INavigationService homeNavigationService)
        {
            this.userStore = userStore;
            this.homeNavigationService = homeNavigationService;
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
                homeNavigationService.Navigate();
            }
        }
        #endregion
    }
}
