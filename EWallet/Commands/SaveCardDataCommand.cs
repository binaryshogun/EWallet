using EWallet.Components;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.Stores;
using EWallet.ViewModels;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    public sealed class SaveCardDataCommand : CommandBase
    {
        private readonly UserStore userStore;
        private readonly CardViewModel cardViewModel;

        public SaveCardDataCommand(UserStore userStore, CardViewModel cardViewModel)
        {
            this.userStore = userStore;
            this.cardViewModel = cardViewModel;
        }

        public override void Execute(object parameter)
            => Task.Run(SaveCardDataInDatabase);

        private async Task SaveCardDataInDatabase()
        {
            cardViewModel.IsDataBeingSaved = true;
            try
            {
                using (var database = new WalletEntities())
                {
                    string encryptedCardNumber = EncryptionHelper.Encrypt(cardViewModel.CardNumber);
                    int.TryParse(cardViewModel.ValidThruMonth, out int month);
                    int.TryParse(cardViewModel.ValidThruYear, out int year);

                    var card = await database
                        .Card.FirstOrDefaultAsync(c =>
                        c.UserID == userStore.CurrentUser.ID
                        && c.Number == encryptedCardNumber);
                    if (card == null)
                    {
                        card = new Card()
                        {
                            Number = encryptedCardNumber,
                            ValidThru = new DateTime(year + 2000, month, 1),
                            UserID = userStore.CurrentUser.ID
                        };
                    }
                    else
                    {
                        card.ValidThru = new DateTime(year + 2000, month, 1);
                    }
                    database.Card.AddOrUpdate(card);
                    await database.SaveChangesAsync();
                }
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally
            {
                cardViewModel.IsDataBeingSaved = false;
            }
        }
    }
}
