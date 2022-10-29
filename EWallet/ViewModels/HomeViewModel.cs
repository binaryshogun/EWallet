using EWallet.Commands;
using EWallet.Services;
using System.Windows.Input;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel домашней страницы.
    /// </summary>
    public sealed class HomeViewModel : ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="HomeViewModel"/>.
        /// </summary>
        /// <param name="authorizationNavigationService">
        /// <see cref="INavigationService"/>, совершающий переход
        /// на <see cref="AuthorizationViewModel"/>.</param>
        /// <param name="registrationNavigationService">
        /// <see cref="INavigationService"/>, совершающий переход
        /// на <see cref="RegistrationViewModel"/>.</param>
        public HomeViewModel(INavigationService authorizationNavigationService,
            INavigationService registrationNavigationService)
        {
            NavigateAuthorizationCommand = new NavigateCommand(
                authorizationNavigationService);
            NavigateRegistrationCommand = new NavigateCommand(
                registrationNavigationService);
        }
        #endregion

        #region Commands
        /// <summary>
        /// Команда перехода на страницу регистрации.
        /// </summary>
        public ICommand NavigateRegistrationCommand { get; }
        /// <summary>
        /// Команда перехода на страницу авторизации.
        /// </summary>
        public ICommand NavigateAuthorizationCommand { get; }
        #endregion

        #region Methods
        /// <inheritdoc cref="ViewModelBase.Dispose"/>
        public override void Dispose() => base.Dispose();
        #endregion
    }
}
