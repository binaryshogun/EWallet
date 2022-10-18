using EWallet.Components;
using EWallet.Models;
using EWallet.Stores;
using EWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    public class TransferCommand : CommandBase
    {
        private readonly UserStore userStore;
        private readonly TransferViewModel transferViewModel;

        public TransferCommand(UserStore userStore, TransferViewModel transferViewModel)
        {
            this.userStore = userStore;
            this.transferViewModel = transferViewModel;
        }

        public override void Execute(object parameter) 
            => Task.Run(ProvideTransfer);

        private async Task ProvideTransfer()
        {
            transferViewModel.IsOperationBeingProvided = true;

            try
            {
                using (var dataBase = new WalletEntities())
                {
                    var cardNumber = GetHash(transferViewModel.CardNumber, 16);

                    //разбить подобные await на отдельные методы Task<Card> и т.д.
                    var card = await dataBase
                        .Card
                        .AsNoTracking()
                        .Where(c => c.Number == cardNumber)
                        .FirstOrDefaultAsync();

                    if (card == null)
                        throw new Exception("Не найден пользователь с введёнными данными карты!");

                    if (card.UserID == userStore.CurrentUser.ID)
                        throw new Exception("Вы не можете перевести деньги самому себе!");

                    double.TryParse(transferViewModel.OperationSum + transferViewModel.Comission, out double sum);
                    if (userStore.CurrentUser.Balance < sum)
                        throw new Exception("Недостаточно денег на счёте!");

                    sum += double.Parse(transferViewModel.Comission);

                    var user = await dataBase
                        .User
                        .AsNoTracking()
                        .Where(u => u.ID == userStore.CurrentUser.ID)
                        .FirstAsync();
                    userStore.CurrentUser.Balance -= sum;
                    user.Balance -= sum;
                    dataBase.User.AddOrUpdate(user);
                    var service = await dataBase
                        .Service
                        .AsNoTracking()
                        .Where(o => o.ID == 1)
                        .FirstAsync();

                    var operation = new Operation()
                    {
                        Number = dataBase.Operation.Last().Number + 1,
                        Date = DateTime.Now,
                        Sum = sum,
                        UserID = user.ID,
                        ToUserID = card.UserID,
                        ServiceID = service.ID
                    };

                    dataBase.Operation.Add(operation);
                    await dataBase.SaveChangesAsync();
                }
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally
            {
                transferViewModel.IsOperationBeingProvided = false;
            }
        }

        /// <summary>
        /// Хэширует строку, используя криптографический алгоритм SHA-1.
        /// </summary>
        /// <param name="password">Пароль для хэширования.</param>
        /// <param name="length">Длина возвращаемой строки.</param>
        /// <returns>Хэшированный пароль.</returns>
        public static string GetHash(string password, int length)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2"))).Substring(0, length);
            }
        }
    }
}
