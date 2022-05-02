using _119_Karpovich.ViewModels;
using System;

namespace _119_Karpovich.Stores
{
    /// <summary>
    /// Навигационное хранилище для хранения данных о текущей VieModel.
    /// </summary>
    public class NavigationStore
    {
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
                currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        /// <summary>
        /// Метод, обрабатывающий изменение текущей ViewModel.
        /// </summary>
        private void OnCurrentViewModelChanged() => CurrentViewModelChanged?.Invoke();

        /// <summary>
        /// Событие при изменении CurrentViewModel.
        /// </summary>
        public event Action CurrentViewModelChanged;

        private ViewModelBase currentViewModel;
    }
}
