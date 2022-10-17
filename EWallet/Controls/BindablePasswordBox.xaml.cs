using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace EWallet.Controls
{
    /// <summary>
    /// PasswordBox с возможностью привязки данных.
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        #region Fields
        private bool _isPasswordChanging;
        public event RoutedEventHandler PasswordChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует PasswordBox с возможностью привязки.
        /// </summary>
        public BindablePasswordBox() => InitializeComponent();
        #endregion

        #region Methods
        /// <summary>
        /// Метод, обновляющий свойство пароля PasswordBox из BindablePasswordBox.
        /// </summary>
        private void UpdatePassword()
        {
            if (!_isPasswordChanging)
                passwordBox.Password = Password;
        }

        /// <summary>
        /// Метод, обновляющий свойство фона PasswordBox из BindablePasswordBox.
        /// </summary>
        public void UpdateBackground() 
            => passwordBox.Background = Background;

        /// <summary>
        /// Метод, обновляющий свойство максимальной длины PasswordBox из BindablePasswordBox.
        /// </summary>
        private void UpdateMaxLength() 
            => passwordBox.MaxLength = MaxLength;
        #endregion

        #region MethodsRoutedEventHandlers
        #region EventHandlers working with PasswordProperty
        /// <inheritdoc cref="RoutedEventHandler"/>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordHint.Visibility = passwordBox.Password.Length == 0 ? Visibility.Visible : Visibility.Collapsed;
            passwordBox.Background = passwordBox.Password.Length == 0 ? null : new SolidColorBrush(Colors.White);

            _isPasswordChanging = true;
            Password = passwordBox.Password;
            _isPasswordChanging = false;
            PasswordChanged?.Invoke(this, new RoutedEventArgs());
        }

        /// <inheritdoc cref="PropertyChangedCallback"/>
        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdatePassword();
            }
        }
        #endregion

        #region EventHandlers working with BackgroundProperty
        /// <inheritdoc cref="RoutedEventHandler"/>
        private void PasswordBox_BackgroundChanged(object sender, RoutedEventArgs e) 
            => Background = passwordBox.Background;

        /// <inheritdoc cref="PropertyChangedCallback"/>
        private static void BackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdateBackground();
            }
        }
        #endregion

        #region EventHandlers working with MaxLengthProperty
        /// <inheritdoc cref="RoutedEventHandler"/>
        private void PasswordBox_MaxLengthChanged(object sender, RoutedEventArgs e) 
            => MaxLength = passwordBox.MaxLength;

        /// <inheritdoc cref="PropertyChangedCallback"/>
        private static void MaxLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdateMaxLength();
            }
        }
        #endregion
        #endregion

        #region Properties
        /// <inheritdoc cref="PasswordBox.Password"/>
        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        /// <inheritdoc cref="TextBlock.Background"/>
        public new Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        /// <inheritdoc cref="PasswordBox.MaxLength"/>
        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }
        #endregion

        #region DependencyProperties
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PasswordPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    BackgroundPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        // Using a DependencyProperty as the backing store for MaxLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(BindablePasswordBox), 
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    MaxLengthPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));
        #endregion
    }
}
