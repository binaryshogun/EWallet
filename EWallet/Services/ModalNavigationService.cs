using EWallet.Stores;
using EWallet.ViewModels;
using System;

namespace EWallet.Services
{
    public sealed class ModalNavigationService<TViewModel> : INavigationService
        where TViewModel : ViewModelBase
    {
        #region Fields
        private readonly ModalNavigationStore modalNavigationStore;
        private readonly Func<TViewModel> createViewModel;
        #endregion

        #region Constructors
        public ModalNavigationService(ModalNavigationStore modalNavigationStore, Func<TViewModel> createViewModel)
        {
            this.modalNavigationStore = modalNavigationStore;
            this.createViewModel = createViewModel;
        }
        #endregion

        #region Methods
        public void Navigate()
            => modalNavigationStore.CurrentViewModel = createViewModel?.Invoke();
        #endregion
    }
}
