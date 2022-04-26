using _119_Karpovich.Stores;

namespace _119_Karpovich.ViewModels
{
    class DisplayViewModel : ViewModelBase
    {
        public DisplayViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this.navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public ViewModelBase CurrentViewModel => navigationStore.CurrentViewModel;

        private readonly NavigationStore navigationStore;
    }
}
