using EWallet.Stores;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel для MainWindow.xaml.
    /// </summary>
    class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Основной класс ViewModel.
        /// </summary>
        /// <param name="navigationStore">Навигационное хранилище.</param>
        public MainViewModel(NavigationStore navigationStore, ModalNavigationStore modalNavigationStore)
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
