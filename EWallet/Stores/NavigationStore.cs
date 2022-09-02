using EWallet.ViewModels;
using System;

namespace EWallet.Stores
{
    /// <summary>
    /// Навигационное хранилище для хранения данных о текущей VieModel.
    /// </summary>
    public class NavigationStore
    {
        private ViewModelBase currentViewModel;
        /// <summary>
        /// Текущая ViewModel.
        /// </summary>
        /// <value>
        /// Содержит текущую ViewModel,
        /// отображаемую при работе приложения
        /// в главном окне.
        /// </value>
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
        /// Событие при изменении CurrentViewModel.
        /// </summary>
        public event Action CurrentViewModelChanged;

        /// <summary>
        /// Метод, обрабатывающий изменение текущей ViewModel.
        /// </summary>
        private void OnCurrentViewModelChanged() => CurrentViewModelChanged?.Invoke();
    }
}
