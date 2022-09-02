using EWallet.Services;
using EWallet.ViewModels;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда навигации.
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel для навигации.</typeparam>
    internal class NavigateCommand : CommandBase
    {
        #region Fields
        private readonly INavigationService navigationService;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует команду навигации.
        /// </summary>
        /// <param name="navigationService">Сервис навигации, привязанный к TViewModel.</param>
        public NavigateCommand(INavigationService navigationService) 
            => this.navigationService = navigationService;
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter) 
            => navigationService.Navigate();
        #endregion
    }
}
