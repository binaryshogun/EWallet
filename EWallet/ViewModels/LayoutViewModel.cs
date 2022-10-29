namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel для отображения страницы 
    /// с навигационной панелью и контентом.
    /// </summary>
    public sealed class LayoutViewModel : ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр 
        /// класса <see cref="LayoutViewModel"/>.
        /// </summary>
        /// <param name="navigationBarViewModel">
        /// ViewModel навигационной панели для отображения.</param>
        /// <param name="contentViewModel">
        /// ViewModel для отображения контента.</param>
        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, ViewModelBase contentViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;
        }
        #endregion

        #region Properties
        /// <summary>
        /// ViewModel навигационной панели.
        /// </summary>
        public NavigationBarViewModel NavigationBarViewModel { get; }
        /// <summary>
        /// ViewModel контента.
        /// </summary>
        public ViewModelBase ContentViewModel { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Освобождает ресурсы <see cref="NavigationBarViewModel"/> 
        /// и <see cref="ContentViewModel"/>.
        /// </summary>
        public override void Dispose()
        {
            NavigationBarViewModel.Dispose();
            ContentViewModel.Dispose();

            base.Dispose();
        }
        #endregion
    }
}
