using System.Diagnostics;
using System.Windows;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда выхода из приложения.
    /// </summary>
    public sealed class ExitAppCommand : CommandBase
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ExitAccountCommand"/>.
        /// </summary>
        public ExitAppCommand() { }

        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter)
        {
            if (MessageBox.Show("Вы действительно хотите выйти?",
                    "Подтверждение", 
                        MessageBoxButton.YesNo, 
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
                Process.GetCurrentProcess().Kill();
        }
    }
}
