using EWallet.Models;

namespace EWallet.Commands
{
    public class CardStore
    {
        #region Fields
        private Card currentCard;
        #endregion

        #region Properties
        /// <summary>
        /// Текущая карта.
        /// </summary>
        /// <value>
        /// Хранит текущую карту, установленную в команде NavigateCardCommand.
        /// </value>
        public Card CurrentCard
        {
            get => currentCard;
            set => currentCard = value;
        }
        #endregion
    }
}