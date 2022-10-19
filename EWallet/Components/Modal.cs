using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EWallet.Components
{
    /// <summary>
    /// <components:Modal/>
    /// </summary>
    public sealed class Modal : ContentControl
    {
        #region Constructors
        static Modal()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Modal), new FrameworkPropertyMetadata(typeof(Modal)));
            BackgroundProperty.OverrideMetadata(typeof(Modal), new FrameworkPropertyMetadata(CreateDefaultBackground()));
        }
        #endregion

        #region Properties
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }
        #endregion

        #region Dependency properties
        public static readonly DependencyProperty IsOpenProperty =
                DependencyProperty.Register("IsOpen", typeof(bool), typeof(Modal), new PropertyMetadata(false));
        #endregion

        #region Methods
        private static object CreateDefaultBackground() => new SolidColorBrush(Colors.Black)
        {
            Opacity = 0.3
        };
        #endregion
    }
}
