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
    /// Команда проведения операции пополнения баланса пользователя.
    /// </summary>
    public sealed class RefillCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly RefillViewModel refillViewModel;
        private readonly INavigationService accountNavigationService;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RefillCommand"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>, 
        /// содержащий данные о текущем пользователе.</param>
        /// <param name="refillViewModel"><see cref="RefillViewModel"/>,
        /// содержащая данные для проведения операции.</param>
        /// <param name="accountNavigationService"><see cref="INavigationService"/>,
        /// совершающий переход на <see cref="AccountViewModel"/>.</param>
        public RefillCommand(UserStore userStore, RefillViewModel refillViewModel, INavigationService accountNavigationService)
        {
            this.userStore = userStore;
            this.refillViewModel = refillViewModel;
            this.accountNavigationService = accountNavigationService;
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter)
            => Task.Run(ProvideRefill);

        /// <summary>
        /// Осуществляет проведение операции пополнения баланса пользователя.
        /// </summary>
        /// <returns>Задача <see cref="Task"/>, представляющая асинхронную операцию.</returns>
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

                    Service service = await OperationsHelper.FetchServiceByName(database, "Пополнение баланса");
                    Operation operation = OperationsHelper.GenerateSingleUserOperation(database, user, sum, service);

                    database.User.AddOrUpdate(user);
                    database.Operation.Add(operation);
                    await database.SaveChangesAsync();
                }
            }
            catch (Exception e) 
            { 
                ErrorMessageBox.Show(e); 
            }
            finally
            {
                refillViewModel.IsOperationBeingProvided = false;
                accountNavigationService?.Navigate();
            }
        }

        /// <summary>
        /// Сохраняет данные о карте в базе данных.
        /// </summary>
        /// <param name="database">Экземпляр базы данных <see cref="WalletEntities"/>.</param>
        /// <returns>Задача <see cref="Task"/>, представляющая асинхронную операцию.</returns>
        private async Task SaveCardData(WalletEntities database)
        {
            string cardNumber = EncryptionHelper.Encrypt(refillViewModel.CardNumber);
            Card card = await OperationsHelper.FetchCardByNumber(database, cardNumber);
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
        /// <summary>
        /// Устанавливает сумму операции с учётом комиссии.
        /// </summary>
        /// <returns>Сумма операции с прибавленным комиссионным взносом.</returns>
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
