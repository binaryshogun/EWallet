using EWallet.Components;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.Stores;
using EWallet.ViewModels;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда сохранения данных карты.
    /// </summary>
    public sealed class SaveCardDataCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly CardViewModel cardViewModel;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SaveCardDataCommand"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>,
        /// содержащий данные о текущем пользователе.</param>
        /// <param name="cardViewModel"><see cref="CardViewModel"/>,
        /// содержащая данные о карте для сохранения.</param>
        public SaveCardDataCommand(UserStore userStore, CardViewModel cardViewModel)
        {
            this.userStore = userStore;
            this.cardViewModel = cardViewModel;
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter)
            => Task.Run(SaveCardDataInDatabase);

        /// <summary>
        /// Сохраняет данные о карте в базе данных.
        /// </summary>
        /// <returns>Задача <see cref="Task"/>, представляющая асинхронную операцию.</returns>
        private async Task SaveCardDataInDatabase()
        {
            cardViewModel.IsDataBeingSaved = true;
            try
            {
                using (var database = new WalletEntities())
                {
                    string encryptedCardNumber = EncryptionHelper.Encrypt(cardViewModel.CardNumber);
                    int.TryParse(cardViewModel.ValidThruMonth, out int month);
                    int.TryParse(cardViewModel.ValidThruYear, out int year);

                    var card = await database
                        .Card.FirstOrDefaultAsync(c => c.Number == encryptedCardNumber);
                    if (card == null)
                    {
                        card = new Card()
                        {
                            Number = encryptedCardNumber,
                            ValidThru = new DateTime(year + 2000, month, 1),
                            UserID = userStore.CurrentUser.ID
                        };
                    }
                    else
                    {
                        if (card.UserID == userStore.CurrentUser.ID)
                            throw new CardAlreadyInSystemException("Карта уже привязана к другому пользователю");
                        card.ValidThru = new DateTime(year + 2000, month, 1);
                    }
                    database.Card.AddOrUpdate(card);
                    await database.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                ErrorMessageBox.Show(e);
            }
            finally
            {
                cardViewModel.IsDataBeingSaved = false;
            }
        }
        #endregion
    }

    /// <summary>
    /// Исключение, препятствующее добавлению карты в систему, 
    /// когда карта привязана к другому пользователю.
    /// </summary>
    public sealed class CardAlreadyInSystemException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CardAlreadyInSystemException"/>.
        /// </summary>
        public CardAlreadyInSystemException()
        {
        }

        /// <summary>
        /// <inheritdoc cref="CardAlreadyInSystemException.CardAlreadyInSystemException"/>
        /// </summary>
        /// <param name="message">Сообщение исключения.</param>
        public CardAlreadyInSystemException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="CardAlreadyInSystemException(string)"/>
        /// <param name="inner">Внутреннее исключение</param>
        public CardAlreadyInSystemException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
