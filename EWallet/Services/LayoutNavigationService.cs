using EWallet.Stores;
using EWallet.ViewModels;
using System;

namespace EWallet.Services
{
    public sealed class LayoutNavigationService<TViewModel> : INavigationService 
        where TViewModel : ViewModelBase
    {
        #region Fields
        private readonly NavigationStore navigationStore;
        private readonly Func<NavigationBarViewModel> createNavigationBarViewModel;
        private readonly Func<TViewModel> createViewModel;
        #endregion

        #region Constructors
        public LayoutNavigationService(NavigationStore navigationStore,
            Func<NavigationBarViewModel> createNavigationBarViewModel,
            Func<TViewModel> createViewModel)
        {
            this.navigationStore = navigationStore;
            this.createNavigationBarViewModel = createNavigationBarViewModel;
            this.createViewModel = createViewModel;
        }
        #endregion

        #region Methods
        public void Navigate() 
            => navigationStore.CurrentViewModel = new LayoutViewModel(
                createNavigationBarViewModel?.Invoke(), createViewModel?.Invoke());
        #endregion
    }
}
