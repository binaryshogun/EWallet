using EWallet.Helpers;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EWallet.Behaviors
{
    /// <summary>
    /// Поведение, позволяющее динамически изменять размер шрифта 
    /// в дочерних для <see cref="Grid"/> <see cref="TextBlock"/> элементах.
    /// </summary>
    public class ScaleFontBehavior : Behavior<Grid>
    {
        #region Properties
        /// <summary>
        /// Свойство, хранящее максимальный размер 
        /// шрифта для <see cref="TextBlock"/> элемента.
        /// </summary>
        public double MaxFontSize
        {
            get => (double)GetValue(MaxFontSizeProperty);
            set => SetValue(MaxFontSizeProperty, value);
        }
        #endregion

        #region Dependency properties
        public static readonly DependencyProperty MaxFontSizeProperty = 
            DependencyProperty.Register("MaxFontSize", typeof(double), 
                typeof(ScaleFontBehavior), new PropertyMetadata(20d));
        #endregion

        #region Methods
        /// <summary>
        /// Метод, вызываемый при присоединении 
        /// поведения к элементу <see cref="Grid"/>.
        /// </summary>
        protected override void OnAttached()
        {
            CalculateFontSize();
            AssociatedObject.SizeChanged += (s, e) => CalculateFontSize();
        }
        /// <summary>
        /// Метод, вызываемый при отсоединени 
        /// поведения от <see cref="Grid"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= (s, e) => CalculateFontSize();
            base.OnDetaching();
        }

        /// <summary>
        /// Рассчитывает максимальный размер шрифта 
        /// для <see cref="TextBlock"/> элементов в 
        /// присоединенном <see cref="Grid"/>.
        /// </summary>
        private void CalculateFontSize()
        {
            double fontSize = MaxFontSize;

            List<TextBlock> textBlocks = VisualHelper.FindVisualChildren<TextBlock>(AssociatedObject);

            foreach (var textBlock in textBlocks)
            {
                double rowHeight = GetRowHeight(textBlock);
                ColumnDefinition col = AssociatedObject.ColumnDefinitions[Grid.GetColumn(textBlock)];
                
                double colWidth = col.Width.IsAuto ? double.MaxValue : col.ActualWidth ;
                double colActualWidth = col.ActualWidth == double.MaxValue ? col.ActualWidth : col.ActualWidth * Grid.GetColumnSpan(textBlock);

                Size desiredSize = MeasureText(textBlock);
                GetMarginSize(textBlock, out double widthMargins, out double heightMargins);

                double desiredHeight = desiredSize.Height + heightMargins;
                double desiredWidth = desiredSize.Width + widthMargins;

                if (colWidth < desiredWidth)
                {
                    double factor = (desiredWidth - widthMargins) / (colActualWidth - widthMargins);
                    fontSize = Math.Min(fontSize, MaxFontSize / factor);
                }

                if (rowHeight < desiredHeight)
                {
                    double factor = (desiredHeight - heightMargins) / (AssociatedObject.ActualHeight - heightMargins);
                    fontSize = Math.Min(fontSize, MaxFontSize / factor);
                }
            }

            foreach (var textBlock in textBlocks)
            {
                textBlock.FontSize = fontSize;
            }
        }
        /// <summary>
        /// Рассчитывает размеры свойства 
        /// Margin элемента <see cref="TextBlock"/>.
        /// </summary>
        /// <param name="textBlock"><see cref="TextBlock"/> элемент.</param>
        /// <param name="widthMargins">Общая ширина Margin.</param>
        /// <param name="heightMargins">Общая высота Margin.</param>
        private void GetMarginSize(TextBlock textBlock, out double widthMargins, out double heightMargins)
        {
            widthMargins = textBlock.Margin.Left + textBlock.Margin.Right;
            heightMargins = textBlock.Margin.Top + textBlock.Margin.Bottom;
        }
        /// <summary>
        /// Возвращает высоту строки Row элемента 
        /// <see cref="Grid"/> в которой находится 
        /// элемент <see cref="TextBlock"/>.
        /// </summary>
        /// <param name="textBlock"><see cref="TextBlock"/> элемент.</param>
        /// <returns>Высота строки Row элемента <see cref="Grid"/>.</returns>
        private double GetRowHeight(TextBlock textBlock)
        {
            RowDefinition row = AssociatedObject.RowDefinitions[Grid.GetRow(textBlock)];
            double rowHeight = row.Height.IsAuto ? double.MaxValue : AssociatedObject.ActualHeight;
            return rowHeight;
        }
        /// <summary>
        /// Возвращает размер 
        /// текста элемента <see cref="TextBlock"/>.
        /// </summary>
        /// <param name="textBlock"><see cref="TextBlock"/> элемент.</param>
        /// <returns>Размер текста элемента <see cref="TextBlock"/>.</returns>
        private Size MeasureText(TextBlock textBlock)
        {
            var formattedText = new FormattedText(textBlock.Text, CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
                MaxFontSize, Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }
        #endregion
    }
}
