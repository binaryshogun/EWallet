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
    /// <summary>
    /// Команда, обеспечивающая проведение операции вывода средств.
    /// </summary>
    public sealed class WithdrawCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly WithdrawViewModel withdrawViewModel;
        private readonly INavigationService accountNavigationService;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WithdrawCommand"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>,
        /// содержащий данные о текущем пользователе.</param>
        /// <param name="withdrawViewModel"><see cref="WithdrawViewModel"/>,
        /// содержащий данные для проведения операции вывода.</param>
        /// <param name="accountNavigationService"><see cref="INavigationService"/>,
        /// совершающий переход на <see cref="AccountViewModel"/>.</param>
        public WithdrawCommand(UserStore userStore, WithdrawViewModel withdrawViewModel, INavigationService accountNavigationService)
        {
            this.userStore = userStore;
            this.withdrawViewModel = withdrawViewModel;
            this.accountNavigationService = accountNavigationService;
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter)
            => Task.Run(ProvideWithdraw);
        /// <summary>
        /// Проводит операцию вывода средств.
        /// </summary>
        /// <returns>Задача <see cref="Task"/>, представляющая асинхронную операцию.</returns>
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

                    Service service = await OperationsHelper.FetchServiceByName(database, "Вывод средств");
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
        /// <summary>
        /// Устанавливает сумму операции с учётом комиссии.
        /// </summary>
        /// <returns>Сумма операции с прибавленным комиссионным взносом.</returns>
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
