using EWallet.Stores;
using EWallet.ViewModels;
using System;

namespace EWallet.Services
{
    public sealed class ModalNavigationService<TViewModel> : INavigationService
        where TViewModel : ViewModelBase
    {
        private readonly ModalNavigationStore modalNavigationStore;
        private readonly Func<TViewModel> createViewModel;

        public ModalNavigationService(ModalNavigationStore modalNavigationStore, Func<TViewModel> createViewModel)
        {
            this.modalNavigationStore = modalNavigationStore;
            this.createViewModel = createViewModel;
        }

        public void Navigate() 
            => modalNavigationStore.CurrentViewModel = createViewModel?.Invoke();
    }
}
