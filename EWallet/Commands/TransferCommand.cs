using EWallet.Components;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using EWallet.ViewModels;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда, обеспечивающая проведение перевода средств.
    /// </summary>
    public sealed class TransferCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly TransferViewModel transferViewModel;
        private readonly INavigationService accountNavigationService;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TransferCommand"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>,
        /// содержащий данные о текущем пользователе.</param>
        /// <param name="transferViewModel"><see cref="TransferViewModel"/>,
        /// содержащая данные для проведения перевода.</param>
        /// <param name="accountNavigationService"><see cref="INavigationService"/>,
        /// совершающий переход на <see cref="AccountViewModel"/>.</param>
        public TransferCommand(UserStore userStore, TransferViewModel transferViewModel, INavigationService accountNavigationService)
        {
            this.userStore = userStore;
            this.transferViewModel = transferViewModel;
            this.accountNavigationService = accountNavigationService;
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter) 
            => Task.Run(ProvideTransfer);

        /// <summary>
        /// Проводит операцию перевода между пользователями.
        /// </summary>
        /// <returns>Задача <see cref="Task"/>, представляющая асинхронную операцию.</returns>
        private async Task ProvideTransfer()
        {
            transferViewModel.IsOperationBeingProvided = true;

            try
            {
                using (var database = new WalletEntities())
                {
                    string cardNumber = EncryptionHelper.Encrypt(transferViewModel.CardNumber);
                    Card card = await OperationsHelper.FetchCardByNumber(database, cardNumber);
                    OperationsHelper.CheckCard(card, userStore);
                    
                    User otherUser = GetOtherUser(database, card);
                    User user = await OperationsHelper.FetchUser(database, userStore);

                    double.TryParse(transferViewModel.OperationSum, out double sum);
                    otherUser.Balance += sum;

                    sum = SetSum();
                    OperationsHelper.TryUpdateBalance(user, userStore, -sum);

                    Service service = await OperationsHelper.FetchServiceByName(database, "Перевод");
                    Operation operation = OperationsHelper.GenerateMultiUserOperation(database, card, user, sum, service);

                    database.User.AddOrUpdate(user);
                    database.User.AddOrUpdate(otherUser);
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
                transferViewModel.IsOperationBeingProvided = false;
                accountNavigationService.Navigate();
            }
        }
        /// <summary>
        /// Устанавливает сумму операции с учётом комиссии.
        /// </summary>
        /// <returns>Сумма операции с прибавленным комиссионным взносом.</returns>
        private double SetSum()
        {
            double.TryParse(transferViewModel.OperationSum, out double sum);
            double.TryParse(transferViewModel.Comission, out double comission);
            sum += comission;

            return sum;
        }
        /// <summary>
        /// Получает пользователя, принимающего средства на свой счет.
        /// </summary>
        /// <param name="database">Экземпляр базы данных <see cref="WalletEntities"/>.</param>
        /// <param name="card">Карта, по которой в базе данных находится <see cref="User"/>.</param>
        /// <returns>Пользователь <see cref="User"/>, принимающий средства на счёт при переводе.</returns>
        private User GetOtherUser(WalletEntities database, Card card)
            => database.User.FirstOrDefault(u => u.Login == card.User.Login);
        #endregion
    }
}
