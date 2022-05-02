using _119_Karpovich.Stores;

namespace _119_Karpovich.ViewModels
{
    /// <summary>
    /// ViewModel для MainWindow.xaml.
    /// </summary>
    class DisplayViewModel : ViewModelBase
    {
        /// <summary>
        /// Инициализирует объект класса DisplayViewModel.
        /// </summary>
        /// <param name="navigationStore">Навигационное хранилище.</param>
        public DisplayViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this.navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        /// <summary>
        /// Метод, вызываемый при смене CurrentViewModel.
        /// </summary>
        private void OnCurrentViewModelChanged() => OnPropertyChanged(nameof(CurrentViewModel));

        /// <summary>
        /// Текущая ViewModel.
        /// </summary>
        public ViewModelBase CurrentViewModel => navigationStore.CurrentViewModel;
        
        private readonly NavigationStore navigationStore;
    }
}
