using EWallet.Stores;
using EWallet.ViewModels;
using System;

namespace EWallet.Services
{
    public class LayoutNavigationService<TViewModel> : INavigationService 
        where TViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;
        private readonly Func<NavigationBarViewModel> createNavigationBarViewModel;
        private readonly Func<TViewModel> createViewModel;

        public LayoutNavigationService(NavigationStore navigationStore, Func<NavigationBarViewModel> createNavigationBarViewModel, Func<TViewModel> createViewModel)
        {
            this.navigationStore = navigationStore;
            this.createNavigationBarViewModel = createNavigationBarViewModel;
            this.createViewModel = createViewModel;
        }

        public void Navigate() 
            => navigationStore.CurrentViewModel = new LayoutViewModel(
                createNavigationBarViewModel?.Invoke(), createViewModel?.Invoke());
    }
}
