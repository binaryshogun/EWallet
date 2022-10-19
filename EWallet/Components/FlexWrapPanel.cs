using System.Windows;
using System.Windows.Controls;

namespace EWallet.Components
{
    public sealed class FlexWrapPanel : WrapPanel
    {
        #region Properties
        public double RequestItemWidth
        {
            get => (double)GetValue(RequestItemWidthProperty);
            set => SetValue(RequestItemWidthProperty, value);
        }
        public double RequestItemHeight
        {
            get => (double)GetValue(RequestItemHeightProperty);
            set => SetValue(RequestItemHeightProperty, value);
        }
        #endregion

        #region Dependency properties
        public static readonly DependencyProperty RequestItemWidthProperty =
            DependencyProperty.Register("RequestItemWidth", typeof(double), typeof(FlexWrapPanel), new PropertyMetadata(double.NaN));

        public static readonly DependencyProperty RequestItemHeightProperty =
            DependencyProperty.Register("RequestItemHeight", typeof(double), typeof(FlexWrapPanel), new PropertyMetadata(double.NaN));
        #endregion

        #region Methods
        protected override Size MeasureOverride(Size constraint)
        {
            ProcessRequestedWidth(constraint);
            ProcessRequestedHeight(constraint);

            return base.MeasureOverride(constraint);
        }

        private void ProcessRequestedWidth(Size constraint)
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
        }
        private void ProcessRequestedHeight(Size constraint)
        {
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
        }
        #endregion
    }
}
