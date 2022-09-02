using EWallet.Stores;
using EWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWallet.Services
{
    public class ModalNavigationService<TViewModel> : INavigationService
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
