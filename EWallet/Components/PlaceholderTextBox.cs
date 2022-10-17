using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EWallet.Components
{
    public class PlaceholderTextBox : TextBox
    {
        public enum LanguageEnum
        {
            ru,
            en
        };

        // Using a DependencyProperty as the backing store for IsEmpty.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey IsEmptyPropertyKey =
            DependencyProperty.RegisterReadOnly("IsEmpty", typeof(bool), typeof(PlaceholderTextBox), new PropertyMetadata(true));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placeholder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceholderTextBox), new PropertyMetadata(""));

        public bool IsEmpty
        {
            get { return (bool)GetValue(IsEmptyProperty); }
            private set { SetValue(IsEmptyPropertyKey, value); }
        }

        public static readonly DependencyProperty IsEmptyProperty = IsEmptyPropertyKey.DependencyProperty;

        public new LanguageEnum Language
        {
            get { return (LanguageEnum)GetValue(LanguageProperty); }
            set { SetValue(LanguageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LanguageOption.  This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty LanguageProperty =
            DependencyProperty.Register("Language", typeof(LanguageEnum), typeof(PlaceholderTextBox), new PropertyMetadata(LanguageEnum.ru));
        
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            IsEmpty = string.IsNullOrEmpty(Text);

            base.OnTextChanged(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            var ch = e.Text.ToCharArray()[0];
            if (Language == LanguageEnum.en)
            {
                if (!((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9') || ch == '_'))
                    e.Handled = true;
            }
            else if (Language == LanguageEnum.ru)
            {
                if (!((ch >= 'А' && ch <= 'Я') || (ch >= 'а' && ch <= 'я') || (ch >= '0' && ch <= '9') || ch == '_'))
                    e.Handled = true;
            }

            e.Handled = false;

            base.OnPreviewTextInput(e);
        }
    }
}
