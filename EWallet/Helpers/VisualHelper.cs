using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;

namespace EWallet.Helpers
{
    public class VisualHelper
    {
        #region Methods
        public static List<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            List<T> children = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var o = VisualTreeHelper.GetChild(obj, i);
                if (o != null)
                {
                    if (o is T typeObject)
                        children.Add(typeObject);

                    children.AddRange(FindVisualChildren<T>(o)); // recursive
                }
            }
            return children;
        }
        public static T FindUpVisualTree<T>(DependencyObject initial) where T : DependencyObject
        {
            DependencyObject current = initial;

            while (current != null && current.GetType() != typeof(T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }
        #endregion
    }
}
