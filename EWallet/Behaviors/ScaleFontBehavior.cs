using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EWallet.Behaviors
{
    public class ScaleFontBehavior : Behavior<Grid>
    {
        public double MaxFontSize { get { return (double)GetValue(MaxFontSizeProperty); } set { SetValue(MaxFontSizeProperty, value); } }
        public static readonly DependencyProperty MaxFontSizeProperty = DependencyProperty.Register("MaxFontSize", typeof(double), typeof(ScaleFontBehavior), new PropertyMetadata(20d));

        protected override void OnAttached()
        {
            CalculateFontSize();
            AssociatedObject.SizeChanged += (s, e) => CalculateFontSize();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= (s, e) => CalculateFontSize();
            base.OnDetaching();
        }

        private void CalculateFontSize()
        {
            double fontSize = MaxFontSize;

            List<TextBlock> tbs = VisualHelper.FindVisualChildren<TextBlock>(AssociatedObject);

            foreach (var tb in tbs)
            {
                RowDefinition row = AssociatedObject.RowDefinitions[Grid.GetRow(tb)];
                double rowHeight = row.Height.IsAuto ? double.MaxValue : AssociatedObject.ActualHeight;

                // get column width (if limited)
                ColumnDefinition col = AssociatedObject.ColumnDefinitions[Grid.GetColumn(tb)];
                double colWidth = col.Width.IsAuto ? double.MaxValue : col.ActualWidth;

                // get desired size with fontsize = MaxFontSize
                Size desiredSize = MeasureText(tb);
                double widthMargins = tb.Margin.Left + tb.Margin.Right;
                double heightMargins = tb.Margin.Top + tb.Margin.Bottom;

                double desiredHeight = desiredSize.Height + heightMargins;
                double desiredWidth = desiredSize.Width + widthMargins;

                // adjust fontsize if text would be clipped horizontally
                if (colWidth < desiredWidth)
                {
                    double factor = (desiredWidth - widthMargins) / (col.ActualWidth - widthMargins);
                    fontSize = Math.Min(fontSize, MaxFontSize / factor);
                }

                // adjust fontsize if text would be clipped vertically
                if (rowHeight < desiredHeight)
                {
                    double factor = (desiredHeight - heightMargins) / (AssociatedObject.ActualHeight - heightMargins);
                    fontSize = Math.Min(fontSize, MaxFontSize / factor);
                }
            }

            // apply fontsize (always equal fontsizes)
            foreach (var tb in tbs)
            {
                tb.FontSize = fontSize;
            }
        }

        // Measures text size of textblock
        private Size MeasureText(TextBlock tb)
        {
            var formattedText = new FormattedText(tb.Text, CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch),
                MaxFontSize, Brushes.Black); // always uses MaxFontSize for desiredSize

            return new Size(formattedText.Width, formattedText.Height);
        }
    }
}
