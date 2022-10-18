using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EWallet.Components
{
    public class PlaceholderTextBox : TextBox
    {
        public enum DateFormatParameters
        {
            Month,
            Year
        };

        public enum Languages
        {
            Ru,
            En
        };

        // Using a DependencyProperty as the backing store for IsEmpty.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey IsEmptyPropertyKey =
            DependencyProperty.RegisterReadOnly("IsEmpty", typeof(bool), typeof(PlaceholderTextBox), new PropertyMetadata(true));

        public PlaceholderTextBox() : base()
        {
            Background = new SolidColorBrush(Colors.White);
        }

        static PlaceholderTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderTextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
        }

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

        public new Languages Language
        {
            get { return (Languages)GetValue(LanguageProperty); }
            set { SetValue(LanguageProperty, value); }
        }
        // Using a DependencyProperty as the backing store for LanguageOption.  This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty LanguageProperty =
            DependencyProperty.Register("Language", typeof(Languages), typeof(PlaceholderTextBox), new PropertyMetadata(Languages.Ru));

        public bool IntFormat
        {
            get { return (bool)GetValue(IntFormatProperty); }
            set 
            {
                if (value == true && (DoubleFormat == true || DateFormat == true))
                {
                    SetValue(DoubleFormatProperty, false);
                    SetValue(DateFormatProperty, false);
                }

                SetValue(IntFormatProperty, value);
            }
        }
        // Using a DependencyProperty as the backing store for IntFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntFormatProperty =
            DependencyProperty.Register("IntFormat", typeof(bool), typeof(PlaceholderTextBox), new PropertyMetadata(false));

        public bool DoubleFormat
        {
            get { return (bool)GetValue(DoubleFormatProperty); }
            set 
            { 
                if (value == true && (IntFormat == true || DateFormat == true))
                {
                    SetValue(IntFormatProperty, false);
                    SetValue(DateFormatProperty, false);
                }

                SetValue(DoubleFormatProperty, value);
            }
        }
        // Using a DependencyProperty as the backing store for DoubleFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DoubleFormatProperty =
            DependencyProperty.Register("DoubleFormat", typeof(bool), typeof(PlaceholderTextBox), new PropertyMetadata(false));

        public bool DateFormat
        {
            get { return (bool)GetValue(DateFormatProperty); }
            set
            {
                if (value == true && (IntFormat == true || DoubleFormat == true))
                {
                    SetValue(IntFormatProperty, false);
                    SetValue(DoubleFormatProperty, false);
                }

                SetValue(DateFormatProperty, value);
            }
        }
        // Using a DependencyProperty as the backing store for DateFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateFormatProperty =
            DependencyProperty.Register("DateFormat", typeof(bool), typeof(PlaceholderTextBox), new PropertyMetadata(false));

        public DateFormatParameters DateFormatParameter
        {
            get { return (DateFormatParameters)GetValue(DateFormatParameterProperty); }
            set { SetValue(DateFormatParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateFormatParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateFormatParameterProperty =
            DependencyProperty.Register("DateFormatParameter", typeof(DateFormatParameters), 
                typeof(PlaceholderTextBox), new PropertyMetadata(DateFormatParameters.Month));

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            IsEmpty = string.IsNullOrEmpty(Text);

            if (IntFormat)
                ExtractInt(Text);
            else if (DoubleFormat)
                ExtractDouble(Text);
            else if (DateFormat)
                ExtractDate(Text);

            base.OnTextChanged(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (DoubleFormat)
            {
                if (e.Text == ",")
                    CaretIndex = Text.Length - 2;
                e.Handled = !IsValidText(Text.Insert(CaretIndex, e.Text).TrimEnd('0'));
            }    

            base.OnPreviewTextInput(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
            if (DoubleFormat)
            {
                if (e.Key == Key.Back)
                    if (CaretIndex == Text.Length - 2)
                    {
                        CaretIndex--;
                        e.Handled = true;
                    }
            }

            base.OnPreviewKeyDown(e);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.SourceDataObject.GetData(typeof(string)) is string text)
            {
                if (!IsValidText(Text.Insert(CaretIndex, text)))
                { e.CancelCommand(); }
            }
            else
            { e.CancelCommand(); }
        }

        private bool IsValidText(string text)
        {
            string pattern = "";
            if (Language == Languages.En)
                pattern = "^[a-z]*$";
            else if (Language == Languages.Ru)
                pattern = "^[А-Я][а-я]*$";

            if (IntFormat)
                pattern = @"^[\d]";
            else if (DoubleFormat)
                pattern = @"^([0-9]*?,?[0-9]*?)$";
            else if (DateFormat)
            {
                if (DateFormatParameter == DateFormatParameters.Month)
                    pattern = @"^(([1-9]|1[012]))$";
                else
                    pattern = @"^([0-9][0-9]?)$";
            }

            return Regex.IsMatch(text, pattern);
        }

        private void ExtractInt(string text)
        {
            text = Regex.Replace(text, @"[^0-9]", string.Empty);
            text = text.TrimStart('0');

            Text = text;
        }

        private void ExtractDouble(string text)
        {
            text = Regex.Replace(text, @"[^0-9,]", string.Empty);
            text = text.TrimStart('0');
            if (text.StartsWith(","))
                text = $"0{text}";
            if (text.EndsWith(","))
            {
                CaretIndex++;
                text = $"{text}0";
            }

            double.TryParse(text, out double result);

            var index = CaretIndex;
            Text = string.Format("{0:f2}", result);
            CaretIndex = index;
        }

        private void ExtractDate(string text)
        {
            text = Regex.Replace(text, @"[^0-9]", string.Empty);
            Text = text.ToString();
        }
    }
}
