using EWallet.Stores;

namespace EWallet.Services
{
    public sealed class CloseModalNavigationService : INavigationService
    {
        #region Fields
        private readonly ModalNavigationStore modalNavigationStore;
        #endregion

        #region Constructors
        public CloseModalNavigationService(ModalNavigationStore modalNavigationStore) 
            => this.modalNavigationStore = modalNavigationStore;
        #endregion

        #region Methods
        public void Navigate() => modalNavigationStore.Close();
        #endregion
    }
}
