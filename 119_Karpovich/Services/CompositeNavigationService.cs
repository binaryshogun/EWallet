using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _119_Karpovich.Services
{
    public class CompositeNavigationService : INavigationService
    {
        private readonly IEnumerable<INavigationService> navigationServices;

        public CompositeNavigationService(params INavigationService[] navigationServices) 
            => this.navigationServices = navigationServices;

        public void Navigate()
        {
            foreach (INavigationService navigationService in navigationServices)
                navigationService.Navigate();
        }
    }
}
