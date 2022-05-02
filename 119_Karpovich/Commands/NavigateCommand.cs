using _119_Karpovich.Services;
using _119_Karpovich.ViewModels;

namespace _119_Karpovich.Commands
{
    /// <summary>
    /// Команда навигации.
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel для навигации.</typeparam>
    internal class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        #region Fields
        private readonly NavigationService<TViewModel> navigationService;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует команду навигации.
        /// </summary>
        /// <param name="navigationService">Сервис навигации, привязанный к TViewModel.</param>
        public NavigateCommand(NavigationService<TViewModel> navigationService) 
            => this.navigationService = navigationService;
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter) => navigationService.Navigate();
        #endregion
    }
}
