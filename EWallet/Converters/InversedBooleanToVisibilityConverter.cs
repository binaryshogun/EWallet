﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EWallet.Converters
{
    public sealed class InversedBooleanToVisibilityConverter : IValueConverter
    {
        #region Methods
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => value is Visibility visibility ? visibility == Visibility.Collapsed : (object)false;
        #endregion
    }
}
