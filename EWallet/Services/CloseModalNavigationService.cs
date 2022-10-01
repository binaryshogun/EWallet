using EWallet.Stores;

namespace EWallet.Services
{
    public sealed class CloseModalNavigationService : INavigationService
    {
        private readonly ModalNavigationStore navigationStore;

        public CloseModalNavigationService(ModalNavigationStore navigationStore) => this.navigationStore = navigationStore;

        public void Navigate() => navigationStore.Close();
    }
}
