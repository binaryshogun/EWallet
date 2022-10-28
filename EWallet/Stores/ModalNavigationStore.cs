using EWallet.ViewModels;
using System;

namespace EWallet.Stores
{
    /// <summary>
    /// Хранилище, содержащее данные 
    /// о модальном окне.
    /// </summary>
    public sealed class ModalNavigationStore
    {
        #region Fields
        private ViewModelBase currentViewModel;
        #endregion

        #region Events
        /// <summary>
        /// Событие при изменении CurrentViewModel.
        /// </summary>
        public event Action CurrentViewModelChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Текущая ViewModel.
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel?.Dispose();
                currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }
        /// <summary>
        /// Показывает, открыто ли модальное окно.
        /// </summary>
        public bool IsOpen 
            => CurrentViewModel != null;
        #endregion

        #region Methods
        /// <summary>
        /// Метод, обрабатывающий изменение текущей ViewModel.
        /// </summary>
        private void OnCurrentViewModelChanged() 
            => CurrentViewModelChanged?.Invoke();
        /// <summary>
        /// Закрывает модальное окно.
        /// </summary>
        public void Close() 
            => CurrentViewModel = null;
        #endregion
    }
}
