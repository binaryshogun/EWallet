using EWallet.Components;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.ViewModels;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    /// <summary>
    /// Комманда удаления карты из сохраненных карт.
    /// </summary>
    public sealed class DeleteCardCommand : CommandBase
    {
        #region Fields
        private readonly CardStore cardStore;
        private readonly CardManagmentViewModel cardManagmentViewModel;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DeleteCardCommand"/>.
        /// </summary>
        /// <param name="cardStore"><see cref="CardStore"/>, 
        /// содержащий сведения о текущей карте.</param>
        /// <param name="cardManagmentViewModel"><see cref="CardManagmentViewModel"/>, 
        /// содержащая данные для управления сохраненными картами.</param>
        public DeleteCardCommand(CardStore cardStore, CardManagmentViewModel cardManagmentViewModel)
        {
            this.cardStore = cardStore;
            this.cardManagmentViewModel = cardManagmentViewModel;
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter)
        {
            if (parameter is string cardNumber)
                Task.Run(() => RemoveCardFromDatabase(cardNumber));
            cardStore.CurrentCard = null;
        }

        /// <summary>
        /// Удаляет карту из сохранённых карт пользователя и базы данных.
        /// </summary>
        /// <param name="cardNumber">Номер карты для удаления из базы данных.</param>
        /// <returns>Задача <see cref="Task"/>, представляющая асинхронную операцию.</returns>
        private async Task RemoveCardFromDatabase(string cardNumber)
        {
            cardManagmentViewModel.AreCardsLoading = true;
            try
            {
                using (var database = new WalletEntities())
                {
                    string encryptedCardNumber = EncryptionHelper.Encrypt(cardNumber);
                    var card = await database
                        .Card
                        .FirstOrDefaultAsync(c =>
                        c.Number == encryptedCardNumber);
                    if (card != null)
                        database.Card.Remove(card);

                    await database.SaveChangesAsync();

                    cardManagmentViewModel.FetchCards();
                }
            }
            catch (Exception e) 
            { 
                ErrorMessageBox.Show(e); 
            }
            finally
            {
                cardManagmentViewModel.AreCardsLoading = false;
            }
        }
        #endregion
    }
}
