using EWallet.Services;
using EWallet.Stores;
using EWallet.ViewModels;
using System.Windows;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда выхода из учётной записи.
    /// </summary>
    public sealed class ExitAccountCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly INavigationService homeNavigationService;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ExitAccountCommand"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>,
        /// содержащий информацию о текущем пользователе.</param>
        /// <param name="homeNavigationService"><see cref="INavigationService"/>, 
        /// совершающий переход на <see cref="HomeViewModel"/>.</param>
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
