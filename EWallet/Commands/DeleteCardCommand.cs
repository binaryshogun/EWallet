using EWallet.Components;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    public class DeleteCardCommand : CommandBase
    {
        private readonly CardManagmentViewModel cardManagmentViewModel;

        public DeleteCardCommand(CardManagmentViewModel cardManagmentViewModel) 
            => this.cardManagmentViewModel = cardManagmentViewModel;

        public override void Execute(object parameter)
        {
            if (parameter is string cardNumber)
                Task.Run(() => RemoveCardFromDatabase(cardNumber));
        }

        private async Task RemoveCardFromDatabase(string cardNumber)
        {
            cardManagmentViewModel.AreCardsLoading = true;
            try
            {
                using (var database = new WalletEntities())
                {
                    string encryptedCardNumber = EncryptionHelper.Encrypt(cardNumber);
                    var card = database
                        .Card
                        .AsNoTracking()
                        .FirstOrDefault(c =>
                        c.Number == encryptedCardNumber);
                    if (card != null)
                        database.Entry(card).State = EntityState.Deleted;

                    await database.SaveChangesAsync();

                    cardManagmentViewModel.SetCards();
                }
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally
            {
                cardManagmentViewModel.AreCardsLoading = false;
            }
        }
    }
}
