using System;

namespace EWallet.Exceptions
{
    /// <summary>
    /// Исключение, вызываемое, если пользователь не найден в системе.
    /// </summary>
    public sealed class UserNotFoundException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserNotFoundException"/>.
        /// </summary>
        public UserNotFoundException()
        {
        }

        /// <summary>
        /// <inheritdoc cref="UserNotFoundException.UserNotFoundException"/>
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public UserNotFoundException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="UserNotFoundException(string)"/>
        /// <param name="inner">Внутреннее исключение</param>
        public UserNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
