using EWallet.Components;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using EWallet.ViewModels;
using System;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    public sealed class RefillCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly RefillViewModel refillViewModel;
        private readonly INavigationService accountNavigationService;
        #endregion

        #region Constructors
        public RefillCommand(UserStore userStore, RefillViewModel refillViewModel, INavigationService accountNavigationService)
        {
            this.userStore = userStore;
            this.refillViewModel = refillViewModel;
            this.accountNavigationService = accountNavigationService;
        }
        #endregion

        #region Methods
        public override void Execute(object parameter)
            => Task.Run(ProvideRefill);

        private async Task ProvideRefill()
        {
            refillViewModel.IsOperationBeingProvided = true;

            try
            {
                using (var database = new WalletEntities())
                {
                    if (refillViewModel.SaveCardData)
                        await SaveCardData(database);

                    User user = await OperationsHelper.FetchUser(database, userStore);
                    double sum = SetSum();
                    OperationsHelper.TryUpdateBalance(user, userStore, sum);

                    Service service = await OperationsHelper.FetchService(database, "Пополнение баланса");
                    Operation operation = OperationsHelper.GenerateSingleUserOperation(database, user, sum, service);

                    database.User.AddOrUpdate(user);
                    database.Operation.Add(operation);
                    await database.SaveChangesAsync();
                }
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally
            {
                refillViewModel.IsOperationBeingProvided = false;
                accountNavigationService?.Navigate();
            }
        }

        private async Task SaveCardData(WalletEntities database)
        {
            string cardNumber = EncryptionHelper.Encrypt(refillViewModel.CardNumber);
            Card card = await OperationsHelper.FetchCard(database, cardNumber);
            if (card == null)
            {
                int.TryParse(refillViewModel.ValidThruYear, out int year);
                int.TryParse(refillViewModel.ValidThruMonth, out int month);
                card = new Card()
                {
                    Number = cardNumber,
                    ValidThru = new DateTime(2000 + year, month, 1),
                    UserID = userStore.CurrentUser.ID
                };
                database.Card.Add(card);
            }
            await database.SaveChangesAsync();
        }

        private double SetSum()
        {
            double.TryParse(refillViewModel.OperationSum, out double sum);
            double.TryParse(refillViewModel.Comission, out double comission);
            sum += comission;

            return sum;
        }
        #endregion
    }
}
