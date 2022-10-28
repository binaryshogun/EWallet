using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace EWallet.Converters
{
    /// <summary>
    /// Конвертер, преобразующий значения <see cref="bool"/> c <see cref="Visibility"/>.
    /// Механизм преобразования обратен встроенному конвертеру
    /// <see cref="BooleanToVisibilityConverter"/>.
    /// </summary>
    public sealed class InversedBooleanToVisibilityConverter : IValueConverter
    {
        #region Methods
        /// <summary>
        /// Преобразует логическое значение в значение перечисления <see cref="Visibility"/>.
        /// </summary>
        /// <param name="value">Логическое значение, которое необходимо преобразовать.</param>
        /// <param name="targetType">Этот параметр не используется.</param>
        /// <param name="parameter">Этот параметр не используется.</param>
        /// <param name="culture">Этот параметр не используется.</param>
        /// <returns><see cref="Visibility.Collapsed"/>, если <paramref name="value"/> является <see langword="true"/>; 
        /// в противном случае — <see cref="Visibility.Visible"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool boolean)
            {
                flag = boolean;
            }
            else if (value is bool?)
            {
                bool? flag2 = (bool?)value;
                flag = flag2.HasValue && flag2.Value;
            }

            return (!flag) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Преобразует значение перечисления <see cref="Visibility"/> в логическое значение.
        /// </summary>
        /// <param name="value">Значение перечисления <see cref="Visibility"/>.</param>
        /// <param name = "targetType" > Этот параметр не используется.</param>
        /// <param name = "parameter" > Этот параметр не используется.</param>
        /// <param name = "culture" > Этот параметр не используется.</param>
        /// <returns><see langword="true"/>, если <paramref name="value"/> является <see cref="Visibility.Collapsed"/>;
        /// в противном случае — <see langword="false"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => value is Visibility visibility ? visibility == Visibility.Collapsed : (object)false;
        #endregion
    }
}
