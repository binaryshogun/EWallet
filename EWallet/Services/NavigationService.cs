using EWallet.Stores;
using EWallet.ViewModels;
using System;

namespace EWallet.Services
{
    /// <summary>
    /// Стандартная реализация сервиса 
    /// навигации для совершения переходов.
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel, 
    /// используемый в качестве отображаемой страницы.</typeparam>
    public sealed class NavigationService<TViewModel> : INavigationService 
        where TViewModel : ViewModelBase
    {
        #region Fields
        private readonly Func<TViewModel> createViewModel;
        private readonly NavigationStore navigationStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NavigationService{TViewModel}"/>.
        /// </summary>
        /// <param name="navigationStore">Навигационное хранилище 
        /// для изменения свойства <see cref="NavigationStore.CurrentViewModel"/> 
        /// результатом действия метода, вызываемого <paramref name="createViewModel"/>.</param>
        /// <param name="createViewModel">Делегат <see cref="Func{TResult}"/> 
        /// для создания <typeparamref name="TViewModel"/> отображения.</param>
        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            this.createViewModel = createViewModel;
            this.navigationStore = navigationStore;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Совершает переход на <typeparamref name="TViewModel"/>.
        /// </summary>
        public void Navigate() => navigationStore.CurrentViewModel = createViewModel();
        #endregion
    }
}
