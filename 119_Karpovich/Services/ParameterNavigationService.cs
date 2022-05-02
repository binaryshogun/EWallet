using _119_Karpovich.Models;
using _119_Karpovich.Stores;
using _119_Karpovich.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _119_Karpovich.Services
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
            => navigationStore.CurrentViewModel = createViewModel?.Invoke(parameter);
    }
}
