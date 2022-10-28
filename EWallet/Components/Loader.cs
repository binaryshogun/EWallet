using System.Windows;
using System.Windows.Controls;

namespace EWallet.Components
{
    /// <summary>
    /// UI-элемент представляющий собой показатель загрузки данных или ожидания.
    /// </summary>
    public sealed class Loader : Control
    {
        #region Properties
        /// <summary>
        /// Показывает, активен ли UI-элемент.
        /// </summary>
        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }
        #endregion

        #region Dependency properties
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(Loader), new PropertyMetadata(false));
        #endregion
    }
}
