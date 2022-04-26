using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace _119_Karpovich.Components
{
    /// <summary>
    /// Логика взаимодействия для BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        public BindablePasswordBox()
        {
            InitializeComponent();
            passwordBox.Background = new SolidColorBrush(Colors.AliceBlue);
            passwordBox.MaxLength = 0;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isPasswordChanging = true;
            Password = passwordBox.Password;
            _isPasswordChanging = false;
            PasswordChanged?.Invoke(this, new RoutedEventArgs());
        }

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdatePassword();
            }
        }

        private void UpdatePassword()
        {
            if (!_isPasswordChanging)
                passwordBox.Password = Password;
        }

        private void PasswordBox_BackgroundChanged(object sender, RoutedEventArgs e)
        {
            Background = passwordBox.Background;
        }

        private static void BackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdateBackground();
            }
        }

        public void UpdateBackground()
        {
            passwordBox.Background = Background;
        }

        private void PasswordBox_MaxLengthChanged(object sender, RoutedEventArgs e)
        {
            MaxLength = passwordBox.MaxLength;
        }

        private static void MaxLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdateMaxLength();
            }
        }

        private void UpdateMaxLength()
        {
            passwordBox.MaxLength = MaxLength;
        }

        public int MaxLength 
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(BindablePasswordBox), 
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    MaxLengthPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    BackgroundPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                    PasswordPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        public event RoutedEventHandler PasswordChanged;
        private bool _isPasswordChanging;
    }
}
