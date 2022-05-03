using _119_Karpovich.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _119_Karpovich.Services
{
    public class CloseModalNavigationService : INavigationService
    {
        private readonly ModalNavigationStore navigationStore;

        public CloseModalNavigationService(ModalNavigationStore navigationStore) => this.navigationStore = navigationStore;

        public void Navigate() => navigationStore.Close();
    }
}
