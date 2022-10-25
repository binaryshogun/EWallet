namespace EWallet.ViewModels
{
    public sealed class LayoutViewModel : ViewModelBase
    {
        #region Constructors
        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, ViewModelBase contentViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;
        }
        #endregion

        #region Properties
        public NavigationBarViewModel NavigationBarViewModel { get; }
        public ViewModelBase ContentViewModel { get; }
        #endregion

        #region Methods
        public override void Dispose()
        {
            NavigationBarViewModel.Dispose();
            ContentViewModel.Dispose();

            base.Dispose();
        }
        #endregion
    }
}
