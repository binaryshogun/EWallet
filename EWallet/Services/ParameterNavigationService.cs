using EWallet.Models;
using EWallet.Stores;
using EWallet.ViewModels;
using System;

namespace EWallet.Services
{
    public sealed class ParameterNavigationService<TParameter, TViewModel>
        where TViewModel : ViewModelBase
        where TParameter : User
    {
        #region Fields
        private readonly NavigationStore navigationStore;
        private readonly Func<TParameter, TViewModel> createViewModel;
        #endregion

        #region Constructors
        public ParameterNavigationService(NavigationStore navigationStore, Func<TParameter, TViewModel> createViewModel)
        {
            this.navigationStore = navigationStore;
            this.createViewModel = createViewModel;
        }
        #endregion

        #region Methods
        public void Navigate(TParameter parameter)
            => navigationStore.CurrentViewModel = createViewModel?.Invoke(parameter);
        #endregion
    }
}
