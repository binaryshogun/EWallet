using EWallet.Models;
using EWallet.Stores;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EWallet.Helpers
{
    public static class OperationsHelper
    {
        #region Methods
        #region Async methods
        public static async Task<Service> FetchService(WalletEntities database, int serviceId)
            => await database.Service.AsNoTracking().Where(o => o.ID == serviceId).FirstAsync();
        public static async Task<User> FetchUser(WalletEntities database, UserStore userStore)
            => await database.User.AsNoTracking()
                .Where(u => u.ID == userStore.CurrentUser.ID).FirstAsync();
        public static async Task<Card> FetchCard(WalletEntities database, string cardNumber)
            => await database.Card.AsNoTracking().Where(c => c.Number == cardNumber).FirstOrDefaultAsync();

        #endregion

        public static void TryUpdateBalance(User user, UserStore userStore, double sum)
        {
            if (userStore.CurrentUser.Balance < sum)
                throw new Exception("Недостаточно денег на счёте!");

            userStore.CurrentUser.Balance -= sum;
            user.Balance -= sum;
        }
        public static Operation GenerateOperation(WalletEntities database, Card card, User user, double sum, Service service)
        {
            Operation operation = new Operation
            {
                Date = DateTime.Now,
                Sum = sum,
                UserID = user.ID,
                ToUserID = card.UserID,
                ServiceID = service.ID
            };

            Operation lastOperation = database.Operation.Last();

            if (lastOperation == null)
                operation.Number = 1;
            else
                operation.Number = lastOperation.Number + 1;

            return operation;
        }
        public static void CheckCard(Card card, UserStore userStore)
        {
            if (card == null)
                throw new Exception("Не найден пользователь с введёнными данными карты!");

            if (card.UserID == userStore.CurrentUser.ID)
                throw new Exception("Вы не можете перевести деньги самому себе!");
        }
        #endregion
    }
}
