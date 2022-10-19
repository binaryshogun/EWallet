using EWallet.Services;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда навигации.
    /// </summary>
    public sealed class NavigateCommand : CommandBase
    {
        #region Fields
        private readonly INavigationService navigationService;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NavigateCommand"/>.
        /// </summary>
        /// <param name="navigationService">Сервис навигации, привязанный к ViewModel.</param>
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
