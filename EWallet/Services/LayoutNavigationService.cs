using EWallet.Stores;
using EWallet.ViewModels;
using System;

namespace EWallet.Services
{
    /// <summary>
    /// Навигационный сервис для отображения 
    /// множественных слоев View.
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel для 
    /// отображения в основной части страницы.</typeparam>
    public sealed class LayoutNavigationService<TViewModel> : INavigationService 
        where TViewModel : ViewModelBase
    {
        #region Fields
        private readonly NavigationStore navigationStore;
        private readonly Func<NavigationBarViewModel> createNavigationBarViewModel;
        private readonly Func<TViewModel> createViewModel;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LayoutNavigationService{TViewModel}"/>.
        /// </summary>
        /// <param name="navigationStore"><see cref="NavigationStore"/>,
        /// содержащий свойство <see cref="NavigationStore.CurrentViewModel"/>.</param>
        /// <param name="createNavigationBarViewModel">Делегат <see cref="Func{TResult}"/>,
        /// возвращающий экземпляр <see cref="NavigationBarViewModel"/> 
        /// для отображения в верхней области программы.</param>
        /// <param name="createViewModel">Делегат <see cref="Func{TResult}"/>,
        /// возвращающий экземпляр <typeparamref name="TViewModel"/>
        /// для отображения в основной области программы.<Fun</param>
        public LayoutNavigationService(NavigationStore navigationStore,
            Func<NavigationBarViewModel> createNavigationBarViewModel,
            Func<TViewModel> createViewModel)
        {
            this.navigationStore = navigationStore;
            this.createNavigationBarViewModel = createNavigationBarViewModel;
            this.createViewModel = createViewModel;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Совершает переход между страницами.
        /// </summary>
        public void Navigate() 
            => navigationStore.CurrentViewModel = new LayoutViewModel(
                createNavigationBarViewModel?.Invoke(), createViewModel?.Invoke());
        #endregion
    }
}
