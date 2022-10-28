using System;
using System.Windows;

namespace EWallet.Components
{
    /// <summary>
    /// MessageBox показывающий сообщения об ошибках и исключениях.
    /// </summary>
    public sealed class ErrorMessageBox
    {
        #region Methods
        /// <summary>
        /// Показывает MessageBox с сообщением об ошибке.
        /// </summary>
        /// <param name="e">Экземпляр <see cref="Exception"/>.</param>
        public static void Show(Exception e) 
            => MessageBox.Show(e.Message, "Ошибка!", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        #endregion
    }
}