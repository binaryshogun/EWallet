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
    public class WithdrawCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly WithdrawViewModel withdrawViewModel;
        private readonly INavigationService accountNavigationService;
        #endregion

        #region Constructors
        public WithdrawCommand(UserStore userStore, WithdrawViewModel withdrawViewModel, INavigationService accountNavigationService)
        {
            this.userStore = userStore;
            this.withdrawViewModel = withdrawViewModel;
            this.accountNavigationService = accountNavigationService;
        }
        #endregion

        #region Methods
        public override void Execute(object parameter)
            => Task.Run(ProvideWithdraw);

        private async Task ProvideWithdraw()
        {
            withdrawViewModel.IsOperationBeingProvided = true;

            try
            {
                using (var database = new WalletEntities())
                {
                    User user = await OperationsHelper.FetchUser(database, userStore);
                    double sum = SetSum();
                    OperationsHelper.TryUpdateBalance(user, userStore, -sum);

                    Service service = await OperationsHelper.FetchService(database, "Вывод средств");
                    Operation operation = OperationsHelper.GenerateSingleUserOperation(database, user, sum, service);

                    database.User.AddOrUpdate(user);
                    database.Operation.Add(operation);
                    await database.SaveChangesAsync();
                }
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally
            {
                withdrawViewModel.IsOperationBeingProvided = false;
                accountNavigationService.Navigate();
            }
        }

        private double SetSum()
        {
            double.TryParse(withdrawViewModel.OperationSum, out double sum);
            double.TryParse(withdrawViewModel.Comission, out double comission);
            sum += comission;

            return sum;
        }
        #endregion
    }
}
