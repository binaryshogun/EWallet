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
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
                DependencyProperty.Register("IsOpen", typeof(bool), typeof(Modal), new PropertyMetadata(false));

        static Modal()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Modal), new FrameworkPropertyMetadata(typeof(Modal)));
            BackgroundProperty.OverrideMetadata(typeof(Modal), new FrameworkPropertyMetadata(CreateDefaultBackground()));
        }

        private static object CreateDefaultBackground() => new SolidColorBrush(Colors.Black)
        {
            Opacity = 0.3
        };
    }
}
