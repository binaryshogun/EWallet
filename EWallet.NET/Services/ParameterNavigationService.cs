using EWallet.NET.Models;
using EWallet.Stores;
using EWallet.ViewModels;
using System;

namespace EWallet.Services
{
    public class ParameterNavigationService<TParameter, TViewModel>
        where TViewModel : ViewModelBase
        where TParameter : User
    {
        private readonly NavigationStore navigationStore;
        private readonly Func<TParameter, TViewModel> createViewModel;

        public ParameterNavigationService(NavigationStore navigationStore, Func<TParameter, TViewModel> createViewModel)
        {
            this.navigationStore = navigationStore;
            this.createViewModel = createViewModel;
        }

        public void Navigate(TParameter parameter) 
            => navigationStore.CurrentViewModel = createViewModel.Invoke(parameter);
    }
}
