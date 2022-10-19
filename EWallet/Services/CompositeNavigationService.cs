using System.Collections.Generic;

namespace EWallet.Services
{
    public sealed class CompositeNavigationService : INavigationService
    {
        #region Fields
        private readonly IEnumerable<INavigationService> navigationServices;
        #endregion

        #region Constructors
        public CompositeNavigationService(params INavigationService[] navigationServices)
            => this.navigationServices = navigationServices;
        #endregion

        #region Methods
        public void Navigate()
        {
            foreach (INavigationService navigationService in navigationServices)
                navigationService.Navigate();
        }
        #endregion
    }
}
