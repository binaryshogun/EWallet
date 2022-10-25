using EWallet.Stores;

namespace EWallet.ViewModels
{
    /// <summary>
    /// Основной класс ViewModel.
    /// </summary>
    public sealed class MainViewModel : ViewModelBase
    {
        #region Fields
        private readonly NavigationStore navigationStore;
        private readonly ModalNavigationStore modalNavigationStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Добавить summary
        /// </summary>
        /// <param name="navigationStore">Навигационное хранилище.</param>
        public MainViewModel(NavigationStore navigationStore, ModalNavigationStore modalNavigationStore)
        {
            this.navigationStore = navigationStore;
            this.modalNavigationStore = modalNavigationStore;
            this.navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            this.modalNavigationStore.CurrentViewModelChanged += OnCurrentModalViewModelChanged;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Текущая ViewModel.
        /// </summary>
        public ViewModelBase CurrentViewModel
            => navigationStore.CurrentViewModel;
        public ViewModelBase CurrentModalViewModel
            => modalNavigationStore.CurrentViewModel;
        public bool IsModalOpen
            => modalNavigationStore.IsOpen;
        #endregion

        #region Methods
        private void OnCurrentModalViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
            OnPropertyChanged(nameof(IsModalOpen));
        }

        /// <summary>
        /// Метод, вызываемый при смене CurrentViewModel.
        /// </summary>
        private void OnCurrentViewModelChanged() 
            => OnPropertyChanged(nameof(CurrentViewModel));
        #endregion
    }
}
