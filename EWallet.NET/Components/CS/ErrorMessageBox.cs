using System;
using System.Windows;

namespace EWallet.Components.CS
{
    public sealed class ErrorMessageBox
    {
        public static void Show(Exception e) 
            => MessageBox.Show(e.Message, "Ошибка!", 
                MessageBoxButton.OK, MessageBoxImage.Error);
    }
}