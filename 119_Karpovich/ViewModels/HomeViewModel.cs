using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _119_Karpovich.ViewModels
{
    /// <summary>
    /// ViewModel домашней страницы.
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(NavigationBarViewModel navigationBarViewModel) => NavigationBarViewModel = navigationBarViewModel;

        public NavigationBarViewModel NavigationBarViewModel { get; }
    }
}
