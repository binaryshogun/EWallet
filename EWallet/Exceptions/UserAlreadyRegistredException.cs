using System;

namespace EWallet.Exceptions
{
    /// <summary>
    /// Исключение, вызываемое, если пользователь уже зарегистрирован в системе.
    /// </summary>
    public sealed class UserAlreadyRegistredException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserAlreadyRegistredException"/>.
        /// </summary>
        public UserAlreadyRegistredException()
        {
        }

        /// <summary>
        /// <inheritdoc cref="UserAlreadyRegistredException.UserAlreadyRegistredException"/>
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public UserAlreadyRegistredException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="UserNotFoundException(string)"/>
        /// <param name="inner">Внутреннее исключение</param>
        public UserAlreadyRegistredException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
