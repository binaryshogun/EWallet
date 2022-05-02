using System;
using System.Windows.Input;

namespace _119_Karpovich.Commands
{
    /// <summary>
    /// Базовый абстрактный класс для создания команд
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        /// <inheritdoc cref="ICommand.CanExecute(object)"/>
        public virtual bool CanExecute(object parameter) => true;

        /// <inheritdoc cref="ICommand.Execute(object)"/>
        public abstract void Execute(object parameter);

        protected void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, new EventArgs());
        
        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler CanExecuteChanged;
    }
}
