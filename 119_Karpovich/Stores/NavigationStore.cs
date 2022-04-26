using _119_Karpovich.ViewModels;
using System;

namespace _119_Karpovich.Stores
{
    public class NavigationStore
    {
        public ViewModelBase CurrentViewModel 
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

        public event Action CurrentViewModelChanged;

        private ViewModelBase currentViewModel;
    }
}
