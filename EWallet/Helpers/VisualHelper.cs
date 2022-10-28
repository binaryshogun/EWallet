using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;

namespace EWallet.Helpers
{
    /// <summary>
    /// Статический класс, содержащий набор
    /// методов, используемый для обхода деревьев
    /// детей и предков визуальных элементов.
    /// </summary>
    public static class VisualHelper
    {
        #region Methods
        /// <summary>
        /// Обходит дерево детей объекта <paramref name="obj"/> и находит элементы типа <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Искомый тип детей.</typeparam>
        /// <param name="obj">Объект <see cref="DependencyObject"/> для поиска детей типа <typeparamref name="T"/>.</param>
        /// <returns><see cref="List{T}"/>, содержащий детей объекта <paramref name="obj"/>.</returns>
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

                    children.AddRange(FindVisualChildren<T>(o));
                }
            }
            return children;
        }
        /// <summary>
        /// Обходит дерево предков объекта <paramref name="obj"/> и находит элемент типа <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Искомый тип предка.</typeparam>
        /// <param name="initial">Объект <see cref="DependencyObject"/> для поиска предка типа <typeparamref name="T"/>.</param>
        /// <returns>Объект типа <typeparamref name="T"/>, являющийся предком <paramref name="initial"/> при наличии;
        /// <see langword="null"/> - при отсутствии.</returns>
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
