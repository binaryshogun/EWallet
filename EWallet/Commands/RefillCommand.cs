using EWallet.Components;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.Stores;
using EWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    internal class RefillCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly RefillViewModel refillViewModel;
        #endregion

        #region Constructors
        public RefillCommand(UserStore userStore, RefillViewModel refillViewModel)
        {
            this.userStore = userStore;
            this.refillViewModel = refillViewModel;
        }
        #endregion

        #region Methods
        public override void Execute(object parameter)
            => Task.Run(ProvideTransfer);

        private async Task ProvideTransfer()
        {
            refillViewModel.IsOperationBeingProvided = true;

            try
            {
                using (var database = new WalletEntities())
                {
                    string cardNumber = HashHelper.GetHash(refillViewModel.CardNumber, 16);
                    Card card = await OperationsHelper.FetchCard(database, cardNumber);
                    OperationsHelper.CheckCard(card, this.userStore);

                    User user = await OperationsHelper.FetchUser(database, this.userStore);
                    double sum = SetSum();
                    OperationsHelper.TryUpdateBalance(user, this.userStore, sum);

                    Service service = await OperationsHelper.FetchService(database, 1);
                    Operation operation = OperationsHelper.GenerateOperation(database, card, user, sum, service);

                    database.User.AddOrUpdate(user);
                    database.Operation.Add(operation);
                    await database.SaveChangesAsync();
                }
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally
            {
                refillViewModel.IsOperationBeingProvided = false;
            }
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
