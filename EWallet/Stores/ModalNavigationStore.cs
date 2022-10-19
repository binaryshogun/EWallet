using EWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWallet.Stores
{
    public class ModalNavigationStore
    {
        #region Fields
        private ViewModelBase currentViewModel;
        #endregion

        #region Events
        /// <summary>
        /// Событие при изменении CurrentViewModel.
        /// </summary>
        public event Action CurrentViewModelChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Текущая ViewModel.
        /// </summary>
        /// <value>
        /// Содержит текущую ViewModel,
        /// отображаемую при работе приложения
        /// в главном окне.
        /// </value>
        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel?.Dispose();
                currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }
        public bool IsOpen 
            => CurrentViewModel != null;
        #endregion

        #region Methods
        /// <summary>
        /// Метод, обрабатывающий изменение текущей ViewModel.
        /// </summary>
        private void OnCurrentViewModelChanged() 
            => CurrentViewModelChanged?.Invoke();
        public void Close() 
            => CurrentViewModel = null;
        #endregion
    }
}
