using EWallet.Components;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    public class NavigateCardCommand : CommandBase
    {
        private readonly CardStore cardStore;
        private readonly INavigationService cardNavigationService;

        public NavigateCardCommand(CardStore cardStore, INavigationService cardNavigationService)
        {
            this.cardStore = cardStore;
            this.cardNavigationService = cardNavigationService;
        }

        public override void Execute(object parameter)
        {
            if (parameter is string cardNumber)
            {
                Task fetchCard = new Task(async () => await FetchCardFromDatabase(cardNumber));
                fetchCard.Start();
                fetchCard.Wait();
            }

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
    }
}
