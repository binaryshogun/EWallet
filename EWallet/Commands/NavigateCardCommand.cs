using EWallet.Components;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.Services;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    public sealed class NavigateCardCommand : CommandBase
    {
        #region Fields
        private readonly CardStore cardStore;
        private readonly INavigationService cardNavigationService;
        #endregion

        #region Constructors
        public NavigateCardCommand(CardStore cardStore, INavigationService cardNavigationService)
        {
            this.cardStore = cardStore;
            this.cardNavigationService = cardNavigationService;
        }
        #endregion

        #region Methods
        public override void Execute(object parameter)
        {
            if (parameter is string cardNumber)
                Task.Run(async () => await FetchCardFromDatabase(cardNumber));

            cardNavigationService?.Navigate();
        }

        private async Task FetchCardFromDatabase(string cardNumber)
        {
            string encryptedCardNumber = EncryptionHelper.Encrypt(cardNumber);
            try
            {
                using (var database = new WalletEntities())
                {
                    var card = await database
                        .Card
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => 
                        c.Number == encryptedCardNumber);

                    if (card != null)
                        cardStore.CurrentCard = card;
                }
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally
            {
                cardNavigationService?.Navigate();
            }
        }
        #endregion
    }
}
