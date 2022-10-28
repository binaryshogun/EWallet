using EWallet.Exceptions;
using EWallet.Models;
using EWallet.Stores;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EWallet.Helpers
{
    /// <summary>
    /// Статический класс, содержащий
    /// набор методов, используемых при проведении
    /// операций.
    /// </summary>
    public static class OperationsHelper
    {
        #region Methods
        #region Async methods
        /// <summary>
        /// Асинхронно получает сущность <see cref="Service"/> 
        /// по свойству <see cref="Service.Name"/> из базы данных.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="serviceName"></param>
        /// <returns>Сущность <see cref="Service"/> из базы данных при наличии; 
        /// в противном случае - <see langword="null"/></returns>
        public static async Task<Service> FetchServiceByName(WalletEntities database, string serviceName)
            => await database.Service.AsNoTracking().FirstOrDefaultAsync(s => s.Name == serviceName);
        /// <summary>
        /// Асинхронно получает сущность <see cref="User"/> аналогичную 
        /// <see cref="UserStore.CurrentUser"/> из базы данных.
        /// </summary>
        /// <param name="database">Экземпляр базы данных <see cref="WalletEntities"/>.</param>
        /// <param name="userStore"></param>
        /// <returns>Сущность <see cref="User"/> из базы данных при наличии; 
        /// в противном случае - <see langword="null"/></returns>
        public static async Task<User> FetchUser(WalletEntities database, UserStore userStore)
            => await database.User.AsNoTracking().FirstOrDefaultAsync(u => u.ID == userStore.CurrentUser.ID);
        /// <summary>
        /// Асинхронно получает сущность <see cref="Card"/> 
        /// по свойству <see cref="Card.Number"/> из базы данных. 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="cardNumber"></param>
        /// <returns>Сущность <see cref="Card"/> из базы данных при наличии; 
        /// в противном случае - <see langword="null"/></returns>
        public static async Task<Card> FetchCardByNumber(WalletEntities database, string cardNumber)
            => await database.Card.AsNoTracking().FirstOrDefaultAsync(c => c.Number == cardNumber);
        #endregion

        /// <summary>
        /// Проводит попытку обновления баланса пользователя 
        /// с добавлением суммы к балансу <paramref name="user"/>.
        /// </summary>
        /// <param name="user">Экземпляр <see cref="User"/> из базы данных.</param>
        /// <param name="userStore"><see cref="UserStore"/>, 
        /// содержащий информацию о текущем пользователе для обновления баланса.</param>
        /// <param name="sum">Сумма для обновления баланса.</param>
        /// <exception cref="InsufficientFundsException">Возникает при 
        /// недостатке средств на балансе пользователя <see cref="UserStore.CurrentUser"/></exception>
        public static bool TryUpdateBalance(User user, UserStore userStore, double sum)
        {
            if (sum < 0 && userStore.CurrentUser.Balance < sum)
                throw new InsufficientFundsException("Недостаточно денег на счёте!");

            userStore.CurrentUser.Balance += sum;
            user.Balance += sum;
            return true;
        }
        /// <summary>
        /// Генерирует сущность <see cref="Operation"/> 
        /// с разными получателем и отправителем.
        /// </summary>
        /// <param name="database">Экземпляр базы данных <see cref="WalletEntities"/>.</param>
        /// <param name="card">Карта для использования свойства <see cref="Card.UserID"/>.</param>
        /// <param name="user">Пользователь для использования свойства <see cref="User.ID"/>.</param>
        /// <param name="sum">Сумма операции.</param>
        /// <param name="service">Экземпляр <see cref="Service"/> для записи в <see cref="Operation.ServiceID"/>.</param>
        /// <returns>Новая сущность <see cref="Operation"/> с разными получателем и отправителем.</returns>
        public static Operation GenerateMultiUserOperation(WalletEntities database, Card card, User user, double sum, Service service)
        {
            Operation operation = new Operation
            {
                Date = DateTime.Now,
                Sum = Math.Round(sum, 2),
                UserID = user.ID,
                ToUserID = card.UserID,
                ServiceID = service.ID
            };

            List<Operation> operations = database.Operation.ToList();

            if (operations.Count == 0)
                operation.Number = 1;
            else
                operation.Number = operations.Last().Number + 1;

            return operation;
        }
        /// <summary>
        /// Генерирует сущность <see cref="Operation"/>
        /// с одним пользователем.
        /// </summary>
        /// <param name="database">Экземпляр базы данных <see cref="WalletEntities"/>.</param>
        /// <param name="user">Пользователь для использования свойства <see cref="User.ID"/>.</param>
        /// <param name="sum">Сумма операции.</param>
        /// <param name="service">Экземпляр <see cref="Service"/> для записи в <see cref="Operation.ServiceID"/>.</param>
        /// <returns>Новая сущность <see cref="Operation"/>.</param>
        /// <returns>Новая сущность <see cref="Operation"/> с одним пользователем.</returns>
        public static Operation GenerateSingleUserOperation(WalletEntities database, User user, double sum, Service service)
        {
            Operation operation = new Operation
            {
                Date = DateTime.Now,
                Sum = Math.Round(sum, 2),
                UserID = user.ID,
                ToUserID = user.ID,
                ServiceID = service.ID
            };

            List<Operation> operations = database.Operation.ToList();

            if (operations.Count == 0)
                operation.Number = 1;
            else
                operation.Number = operations.Last().Number + 1;

            return operation;
        }
        /// <summary>
        /// Проверяет карту на правильно введенные данные.
        /// </summary>
        /// <param name="card">Экземпляр <see cref="Card"/> для проверки карты.</param>
        /// <param name="userStore"><see cref="UserStore"/>, хранящий
        /// данные о пользователе для проверки свойства <see cref="Card.UserID"/>.</param>
        /// <exception cref="UserNotFoundException">Возникает при отсутствии карты в базе данных.</exception>
        /// <exception cref="TransferToYourselfException">Возникает при попытке перевода средств на свой счет.</exception>
        public static void CheckCard(Card card, UserStore userStore)
        {
            if (card == null)
                throw new UserNotFoundException("Не найден пользователь с введёнными данными карты!");

            if (card.UserID == userStore.CurrentUser.ID)
                throw new TransferToYourselfException("Вы не можете перевести деньги самому себе!");
        }
        #endregion
    }
}
