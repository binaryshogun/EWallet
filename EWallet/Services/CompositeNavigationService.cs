using System.Collections.Generic;

namespace EWallet.Services
{
    /// <summary>
    /// Множественный навигационный сервис.
    /// </summary>
    public sealed class CompositeNavigationService : INavigationService
    {
        #region Fields
        private readonly IEnumerable<INavigationService> navigationServices;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CompositeNavigationService"/>.
        /// </summary>
        /// <param name="navigationServices">Массив <see cref="INavigationService"/> элементов. 
        /// Используется для множественных переходов.</param>
        public CompositeNavigationService(params INavigationService[] navigationServices)
            => this.navigationServices = navigationServices;
        #endregion

        #region Methods
        /// <summary>
        /// Совершает множественный переход.
        /// </summary>
        public void Navigate()
        {
            foreach (INavigationService navigationService in navigationServices)
                navigationService.Navigate();
        }
        #endregion
    }
}
