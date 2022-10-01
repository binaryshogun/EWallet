using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EWallet.Controls
{
    public sealed class FlexWrapPanel : WrapPanel
    {
        public double RequestItemWidth
        {
            get { return (double)GetValue(RequestItemWidthProperty); }
            set { SetValue(RequestItemWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RequestItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestItemWidthProperty =
            DependencyProperty.Register("RequestItemWidth", typeof(double), typeof(FlexWrapPanel), new PropertyMetadata(double.NaN));

        protected override Size MeasureOverride(Size constraint)
        {
            if (!double.IsNaN(RequestItemWidth))
            {
                double requestedWidth = RequestItemWidth;
                double panelWidth = constraint.Width;

                if (RequestItemWidth > constraint.Width)
                    requestedWidth = panelWidth;

                foreach (UIElement child in InternalChildren)
                {
                    Thickness margin = (Thickness)child.GetValue(MarginProperty);
                    double width = requestedWidth - margin.Left - margin.Right;
                    width = width < 0 ? 0 : width;
                    child.SetValue(WidthProperty, width);
                }
            }
            return base.MeasureOverride(constraint);
        }
    }
}
