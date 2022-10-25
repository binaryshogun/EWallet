using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace EWallet.Controls
{
    /// <summary>
    /// PasswordBox с возможностью привязки данных.
    /// </summary>
    public sealed partial class BindablePasswordBox : UserControl
    {
        #region Fields
        private bool isPasswordChanging;
        public event RoutedEventHandler PasswordChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует PasswordBox с возможностью привязки.
        /// </summary>
        public BindablePasswordBox()
        {
            InitializeComponent();

            Binding placeholderBinding = new Binding("Placeholder")
            {
                Source = this,
                Mode = BindingMode.TwoWay
            };
            passwordHint.SetBinding(Label.ContentProperty, placeholderBinding);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Метод, обновляющий свойство пароля PasswordBox из BindablePasswordBox.
        /// </summary>
        private void UpdatePassword()
        {
            if (!isPasswordChanging)
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

        private bool IsValidPassword(string text)
        {
            string pattern = @"\w*";

            if (OnlyNumbers)
                pattern = @"^[0-9]*$";

            return Regex.IsMatch(text, pattern);
        }

        private void ExtractInt(string text)
        {
            text = Regex.Replace(text, @"[^0-9]", string.Empty);
            text = text.TrimStart('0');

            Password = text;
        }
        #endregion

        #region MethodsRoutedEventHandlers
        #region EventHandlers working with PasswordProperty
        /// <inheritdoc cref="RoutedEventHandler"/>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordHint.Visibility = passwordBox.Password.Length == 0 
                ? Visibility.Visible : Visibility.Collapsed;
            passwordBox.Background = passwordBox.Password.Length == 0 
                ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Colors.White);

            isPasswordChanging = true;
            if (OnlyNumbers)
                ExtractInt(passwordBox.Password);
            else
            {
                Password = passwordBox.Password;
            }
            isPasswordChanging = false;
            PasswordChanged?.Invoke(this, new RoutedEventArgs());
        }
        /// <inheritdoc cref="TextCompositionEventHandler"/>
        private void PasswordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidPassword(passwordBox.Password + e.Text);

            base.OnPreviewTextInput(e);
        }
        /// <inheritdoc cref="KeyEventHandler"/>
        private void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;

            base.OnPreviewKeyDown(e);
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

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
        public bool OnlyNumbers
        {
            get => (bool)GetValue(OnlyNumbersProperty);
            set => SetValue(OnlyNumbersProperty, value);
        }
        #endregion

        #region DependencyProperties
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PasswordPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        public static new readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    BackgroundPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(BindablePasswordBox), 
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    MaxLengthPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(BindablePasswordBox), new PropertyMetadata(""));

        public static readonly DependencyProperty OnlyNumbersProperty =
            DependencyProperty.Register("OnlyNumbers", typeof(bool), typeof(BindablePasswordBox), new PropertyMetadata(false));
        #endregion
    }
}
