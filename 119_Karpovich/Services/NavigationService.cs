using _119_Karpovich.Stores;
using _119_Karpovich.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _119_Karpovich.Services
{
    public class NavigationService<TViewModel>
        where TViewModel : ViewModelBase
    {
        private readonly Func<TViewModel> _createViewModel;
        private readonly NavigationStore _navigationStore;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _createViewModel = createViewModel;
            _navigationStore = navigationStore;
        }
        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
