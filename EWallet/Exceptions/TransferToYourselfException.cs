using System;

namespace EWallet.Exceptions
{
    /// <summary>
    /// Исключение, вызываемое при переводе денежных средств самому себе.
    /// </summary>
    public sealed class TransferToYourselfException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TransferToYourselfException"/>.
        /// </summary>
        public TransferToYourselfException()
        {
        }

        /// <summary>
        /// <inheritdoc cref="TransferToYourselfException.TransferToYourselfException"/>
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public TransferToYourselfException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="TransferToYourselfException(string)"/>
        /// <param name="inner">Внутреннее исключение</param>
        public TransferToYourselfException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
