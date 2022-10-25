using EWallet.Components;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using EWallet.ViewModels;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    public class TransferCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly TransferViewModel transferViewModel;
        private readonly INavigationService accountNavigationService;
        #endregion

        #region Constructors
        public TransferCommand(UserStore userStore, TransferViewModel transferViewModel, INavigationService accountNavigationService)
        {
            this.userStore = userStore;
            this.transferViewModel = transferViewModel;
            this.accountNavigationService = accountNavigationService;
        }
        #endregion

        #region Methods
        public override void Execute(object parameter) 
            => Task.Run(ProvideTransfer);

        private async Task ProvideTransfer()
        {
            transferViewModel.IsOperationBeingProvided = true;

            try
            {
                using (var database = new WalletEntities())
                {
                    string cardNumber = EncryptionHelper.Encrypt(transferViewModel.CardNumber);
                    Card card = await OperationsHelper.FetchCard(database, cardNumber);
                    OperationsHelper.CheckCard(card, userStore);
                    
                    User otherUser = GetOtherUser(database, card);
                    User user = await OperationsHelper.FetchUser(database, userStore);

                    double.TryParse(transferViewModel.OperationSum, out double sum);
                    otherUser.Balance += sum;

                    sum = SetSum();
                    OperationsHelper.TryUpdateBalance(user, userStore, -sum);

                    Service service = await OperationsHelper.FetchService(database, "Перевод");
                    Operation operation = OperationsHelper.GenerateMultiUserOperation(database, card, user, sum, service);

                    database.User.AddOrUpdate(user);
                    database.User.AddOrUpdate(otherUser);
                    database.Operation.Add(operation);
                    await database.SaveChangesAsync();
                }
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally
            {
                transferViewModel.IsOperationBeingProvided = false;
                accountNavigationService.Navigate();
            }
        }

        private double SetSum()
        {
            double.TryParse(transferViewModel.OperationSum, out double sum);
            double.TryParse(transferViewModel.Comission, out double comission);
            sum += comission;

            return sum;
        }

        private User GetOtherUser(WalletEntities database, Card card)
            => database.User.FirstOrDefault(u => u.Login == card.User.Login);
        #endregion
    }
}
