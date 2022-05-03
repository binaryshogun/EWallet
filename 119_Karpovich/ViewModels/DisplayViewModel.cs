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
        public DisplayViewModel(NavigationStore navigationStore, ModalNavigationStore modalNavigationStore)
        {
            this.navigationStore = navigationStore;
            this.modalNavigationStore = modalNavigationStore;
            this.navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            this.modalNavigationStore.CurrentViewModelChanged += OnCurrentModalViewModelChanged;
        }

        private void OnCurrentModalViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
            OnPropertyChanged(nameof(IsModalOpen));
        }

        /// <summary>
        /// Метод, вызываемый при смене CurrentViewModel.
        /// </summary>
        private void OnCurrentViewModelChanged() => OnPropertyChanged(nameof(CurrentViewModel));

        /// <summary>
        /// Текущая ViewModel.
        /// </summary>
        public ViewModelBase CurrentViewModel => navigationStore.CurrentViewModel;
        public ViewModelBase CurrentModalViewModel => modalNavigationStore.CurrentViewModel;
        public bool IsModalOpen => modalNavigationStore.IsOpen;

        private readonly NavigationStore navigationStore;
        private readonly ModalNavigationStore modalNavigationStore;
    }
}
