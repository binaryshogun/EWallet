using _119_Karpovich.Stores;
using _119_Karpovich.ViewModels;
using System;

namespace _119_Karpovich.Services
{
    public class NavigationService<TViewModel>
        where TViewModel : ViewModelBase
    {
        private readonly Func<TViewModel> createViewModel;
        private readonly NavigationStore navigationStore;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            this.createViewModel = createViewModel;
            this.navigationStore = navigationStore;
        }
        public void Navigate()
        {
            navigationStore.CurrentViewModel = createViewModel();
        }
    }
}
