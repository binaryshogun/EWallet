using System.Windows;
using System.Windows.Controls;

namespace EWallet.Components
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

        public double RequestItemHeight
        {
            get { return (double)GetValue(RequestItemHeightProperty); }
            set { SetValue(RequestItemHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RequestItemHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestItemHeightProperty =
            DependencyProperty.Register("RequestItemHeight", typeof(double), typeof(FlexWrapPanel), new PropertyMetadata(double.NaN));

        protected override Size MeasureOverride(Size constraint)
        {
            if (!double.IsNaN(RequestItemWidth))
            {
                double requestedWidth = RequestItemWidth;
                double panelWidth = constraint.Width;

                if (requestedWidth > panelWidth)
                    requestedWidth = panelWidth;

                foreach (UIElement child in InternalChildren)
                {
                    Thickness margin = (Thickness)child.GetValue(MarginProperty);
                    double width = requestedWidth - margin.Left - margin.Right;
                    width = width < 0 ? 0 : width;
                    child.SetValue(WidthProperty, width);
                }
            }
            
            if (!double.IsNaN(RequestItemHeight))
            {
                double requestedHeight = RequestItemHeight;
                double panelHeight = constraint.Height / 4;

                if (requestedHeight > panelHeight)
                    requestedHeight = panelHeight;

                foreach (UIElement child in InternalChildren)
                {
                    child.SetValue(HeightProperty, requestedHeight);
                }
            }

            return base.MeasureOverride(constraint);
        }
    }
}
