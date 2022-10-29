using EWallet.Stores;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel для <see cref="MainWindow"/>.
    /// </summary>
    public sealed class MainViewModel : ViewModelBase
    {
        #region Fields
        private readonly NavigationStore navigationStore;
        private readonly ModalNavigationStore modalNavigationStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainViewModel"/>.
        /// </summary>
        /// <param name="navigationStore">Навигационное хранилище,
        /// хранящая <see cref="NavigationStore.CurrentViewModel"/>.</param>
        /// <param name="modalNavigationStore">Навигационное хранилище модального окна,
        /// хранящее <see cref="ModalNavigationStore.CurrentViewModel"/>.</param>
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
        /// <summary>
        /// Текущий ViewModel модального окна.
        /// </summary>
        public ViewModelBase CurrentModalViewModel
            => modalNavigationStore.CurrentViewModel;
        /// <summary>
        /// Указывает, открыто ли модальное окно.
        /// </summary>
        public bool IsModalOpen
            => modalNavigationStore.IsOpen;
        #endregion

        #region Methods
        /// <summary>
        /// Оповещает об изменении <see cref="CurrentModalViewModel"/> и <see cref="IsModalOpen"/>.
        /// </summary>
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
