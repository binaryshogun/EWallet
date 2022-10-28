using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System;

namespace EWallet.Components
{
    /// <summary>
    /// TextBox с возможностью установки плейсхолдера.
    /// </summary>
    public sealed class PlaceholderTextBox : TextBox
    {
        #region Enums
        /// <summary>
        /// Содержит параметры для задания свойства <see cref="DateFormat"/>.
        /// </summary>
        public enum DateFormatParameters { Month, Year };
        /// <summary>
        /// Содержит параметры для задания свойства <see cref="Language"/>.
        /// </summary>
        public enum Languages { Ru, En };
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PlaceholderTextBox"/>.
        /// </summary>
        public PlaceholderTextBox() : base() 
            => Background = new SolidColorBrush(Colors.White);

        /// <summary>
        /// Задает статические свойства элемента <see cref="PlaceholderTextBox"/>.
        /// </summary>
        static PlaceholderTextBox() 
            => DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderTextBox), 
                new FrameworkPropertyMetadata(typeof(TextBox)));
        #endregion

        #region Properties
        /// <summary>
        /// Плейсхолдер для отображения.
        /// </summary>
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
        /// <summary>
        /// Указывает, пустое ли свойство <see cref="TextBox.Text"/>.
        /// </summary>
        public bool IsEmpty
        {
            get => (bool)GetValue(IsEmptyProperty);
            private set => SetValue(IsEmptyPropertyKey, value);
        }
        /// <summary>
        /// Язык ввода в <see cref="PlaceholderTextBox"/>.
        /// </summary>
        public new Languages Language
        {
            get => (Languages)GetValue(LanguageProperty);
            set => SetValue(LanguageProperty, value);
        }
        /// <summary>
        /// Указывает, доступны ли только цифры при вводе.
        /// </summary>
        public bool IntFormat
        {
            get => (bool)GetValue(IntFormatProperty);
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
        /// <summary>
        /// Указывает, задан ли формат ввода для <see cref="double"/> чисел.
        /// </summary>
        public bool DoubleFormat
        {
            get => (bool)GetValue(DoubleFormatProperty);
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
        /// <summary>
        /// Указывает, задан ли формат ввода для <see cref="DateTime"/> данных.
        /// </summary>
        public bool DateFormat
        {
            get => (bool)GetValue(DateFormatProperty);
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
        /// <summary>
        /// Параметр для определения формата ввода при активном формате <see cref="DateFormat"/>.
        /// </summary>
        public DateFormatParameters DateFormatParameter
        {
            get => (DateFormatParameters)GetValue(DateFormatParameterProperty);
            set => SetValue(DateFormatParameterProperty, value);
        }
        #endregion

        #region Dependency properties
        private static readonly DependencyPropertyKey IsEmptyPropertyKey =
            DependencyProperty.RegisterReadOnly("IsEmpty", typeof(bool), typeof(PlaceholderTextBox), new PropertyMetadata(true));
        public static readonly DependencyProperty IsEmptyProperty = IsEmptyPropertyKey.DependencyProperty;
        
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceholderTextBox), new PropertyMetadata(""));

        public static new readonly DependencyProperty LanguageProperty =
            DependencyProperty.Register("Language", typeof(Languages), typeof(PlaceholderTextBox), new PropertyMetadata(Languages.Ru));

        public static readonly DependencyProperty IntFormatProperty =
            DependencyProperty.Register("IntFormat", typeof(bool), typeof(PlaceholderTextBox), new PropertyMetadata(false));

        public static readonly DependencyProperty DoubleFormatProperty =
            DependencyProperty.Register("DoubleFormat", typeof(bool), typeof(PlaceholderTextBox), new PropertyMetadata(false));

        public static readonly DependencyProperty DateFormatProperty =
            DependencyProperty.Register("DateFormat", typeof(bool), typeof(PlaceholderTextBox), new PropertyMetadata(false));

        public static readonly DependencyProperty DateFormatParameterProperty =
            DependencyProperty.Register("DateFormatParameter", typeof(DateFormatParameters), 
                typeof(PlaceholderTextBox), new PropertyMetadata(DateFormatParameters.Month));
        #endregion

        #region Methods
        /// <inheritdoc cref="TextBoxBase.OnTextChanged(TextChangedEventArgs)"/>
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
        /// <inheritdoc cref="UIElement.OnPreviewTextInput(TextCompositionEventArgs)"/>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (DoubleFormat)
            {
                if (e.Text == ",")
                    CaretIndex = Text.Length - 2;
                e.Handled = !IsValidText(Text.Insert(CaretIndex, e.Text).TrimEnd('0'));
            }
            else
                e.Handled = !IsValidText(Text.Insert(CaretIndex, e.Text));

            base.OnPreviewTextInput(e);
        }
        /// <inheritdoc cref="TextBoxBase.OnPreviewKeyDown(KeyEventArgs)"/>
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
        /// <summary>
        /// Вызывается при вставке текста в <see cref="PlaceholderTextBox"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Аргументы события <see cref="DataObject.Pasting"/></param>
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
        /// <summary>
        /// Проверяет валидность текста элемента <see cref="PlaceholderTextBox"/> в соответствии со всеми форматами.
        /// </summary>
        /// <param name="text">Текст, проверямый на валидность.</param>
        /// <returns><see langword="true"/> в случае валидности текста, <see langword="false"/> - в обратном случае.</returns>
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
                    pattern = @"^((2?[3-9]?)|(3?0?))$";
            }

            return Regex.IsMatch(text, pattern);
        }
        /// <summary>
        /// Извлекает целое число и помещает в свойство <see cref="TextBox.Text"/>.
        /// </summary>
        /// <param name="text">Текст, содержащий целое число.</param>
        private void ExtractInt(string text)
        {
            text = Regex.Replace(text, @"[^0-9]", string.Empty);
            text = text.TrimStart('0');

            Text = text;
        }
        /// <summary>
        /// Извлекает число формата <see cref="double"/> и помещает в свойство <see cref="TextBox.Text"/>.
        /// </summary>
        /// <param name="text">Текст, содержащий число с плавающей точкой.</param>
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
        /// <summary>
        /// Извлекает дату из текста и помещает в свойство <see cref="TextBox.Text"/>.
        /// </summary>
        /// <param name="text">Текст, содержащий дату.</param>
        private void ExtractDate(string text)
        {
            text = Regex.Replace(text, @"[^0-9]", string.Empty);
            Text = text.ToString();
        }
        #endregion
    }
}
