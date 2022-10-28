using EWallet.Stores;
using EWallet.Components;

namespace EWallet.Services
{
    /// <summary>
    /// Навигационный сервис, позволяющий закрыть окно <see cref="Modal"/>.
    /// </summary>
    public sealed class CloseModalNavigationService : INavigationService
    {
        #region Fields
        private readonly ModalNavigationStore modalNavigationStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CloseModalNavigationService"/>.
        /// </summary>
        /// <param name="modalNavigationStore"><see cref="ModalNavigationStore"/>,
        /// содержащий информацию о <see cref="ModalNavigationStore.CurrentViewModel"/>. 
        /// Используется для вызова метода <see cref="ModalNavigationStore.Close"/>.</param>
        public CloseModalNavigationService(ModalNavigationStore modalNavigationStore) 
            => this.modalNavigationStore = modalNavigationStore;
        #endregion

        #region Methods
        /// <summary>
        /// Закрывает текущее модальное окно.
        /// </summary>
        public void Navigate() => modalNavigationStore.Close();
        #endregion
    }
}
