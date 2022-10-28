using System;

namespace EWallet.Exceptions
{
    /// <summary>
    /// Исключение, вызываемое при попытке проведения 
    /// операции при недостаточном количестве средств на счете.
    /// </summary>
    public sealed class InsufficientFundsException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InsufficientFundsException"/>.
        /// </summary>
        public InsufficientFundsException()
        {
        }

        /// <summary>
        /// <inheritdoc cref="InsufficientFundsException.InsufficientFundsException"/>
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public InsufficientFundsException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="TransferToYourselfException(string)"/>
        /// <param name="inner">Внутреннее исключение</param>
        public InsufficientFundsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
