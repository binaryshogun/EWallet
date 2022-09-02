using EWallet.Stores;
using EWallet.ViewModels;
using System;

namespace EWallet.Services
{
    /// <summary>
    /// Сервис навигации в системе.
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel для создания в качестве отображаемой страницы.</typeparam>
    public class NavigationService<TViewModel> : INavigationService 
        where TViewModel : ViewModelBase
    {
        #region Fields
        private readonly Func<TViewModel> createViewModel;
        private readonly NavigationStore navigationStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует объект NavigationService.
        /// </summary>
        /// <param name="navigationStore">Навигационное хранилище для изменения CurrentViewModel на createViewModel.</param>
        /// <param name="createViewModel">Делегат Func для создания TViewModel отображения.</param>
        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            this.createViewModel = createViewModel;
            this.navigationStore = navigationStore;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Метод, осуществляющий смену CurrentViewModel для
        /// navigationStore и меняющий вид отображаемой страницы.
        /// </summary>
        public void Navigate() => navigationStore.CurrentViewModel = createViewModel();
        #endregion
    }
}
