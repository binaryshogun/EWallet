using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EWallet.Components
{
    /// <summary>
    /// Модальное окно для отображения контента.
    /// </summary>
    public sealed class Modal : ContentControl
    {
        #region Constructors
        /// <summary>
        /// Задает статические свойства компонента <see cref="Modal"/>.
        /// </summary>
        static Modal()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Modal), new FrameworkPropertyMetadata(typeof(Modal)));
            BackgroundProperty.OverrideMetadata(typeof(Modal), new FrameworkPropertyMetadata(CreateDefaultBackground()));
        }
        #endregion

        #region Properties
        /// <summary>
        /// Указывает, открыто ли окно <see cref="Modal"/> в текущий момент.
        /// </summary>
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
        /// <summary>
        /// Создает фон по умолчанию для компонента <see cref="Modal"/>.
        /// </summary>
        /// <returns><see cref="SolidColorBrush"/>, представляющее собой фон по умолчанию для компонента <see cref="Modal"/>.</returns>
        private static object CreateDefaultBackground() => new SolidColorBrush(Colors.Black)
        {
            Opacity = 0.3
        };
        #endregion
    }
}
