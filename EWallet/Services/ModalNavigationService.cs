using EWallet.Stores;
using EWallet.ViewModels;
using System;

namespace EWallet.Services
{
    /// <summary>
    /// Навигационный сервис, используемый
    /// для размещения ViewModel в
    /// свойстве <see cref="ModalNavigationStore.CurrentViewModel"/>.
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel для отображения в модальном окне.</typeparam>
    public sealed class ModalNavigationService<TViewModel> : INavigationService
        where TViewModel : ViewModelBase
    {
        #region Fields
        private readonly ModalNavigationStore modalNavigationStore;
        private readonly Func<TViewModel> createViewModel;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый объект класса <see cref="ModalNavigationService{TViewModel}"/>.
        /// </summary>
        /// <param name="modalNavigationStore"><see cref="ModalNavigationStore"/>,
        /// хранящий данные о <see cref="ModalNavigationStore.CurrentViewModel"/>.</param>
        /// <param name="createViewModel">Делегат <see cref="Func{TResult}"/>,
        /// возвращающий <typeparamref name="TViewModel"/> для отображения.</param>
        public ModalNavigationService(ModalNavigationStore modalNavigationStore, Func<TViewModel> createViewModel)
        {
            this.modalNavigationStore = modalNavigationStore;
            this.createViewModel = createViewModel;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Совершает переход и отображение <typeparamref name="TViewModel"/> в модальном окне.
        /// </summary>
        public void Navigate()
            => modalNavigationStore.CurrentViewModel = createViewModel?.Invoke();
        #endregion
    }
}
