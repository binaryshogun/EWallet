using System.Collections.Generic;

namespace EWallet.Services
{
    public sealed class CompositeNavigationService : INavigationService
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
