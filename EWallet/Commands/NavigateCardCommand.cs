using EWallet.Components;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.Services;
using EWallet.ViewModels;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда перехода на страницу добавления или редактирования карты.
    /// </summary>
    public sealed class NavigateCardCommand : CommandBase
    {
        #region Fields
        private readonly CardStore cardStore;
        private readonly INavigationService cardNavigationService;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NavigateCardCommand"/>.
        /// </summary>
        /// <param name="cardStore"><see cref="CardStore"/>,
        /// содержащий информацию о текущей карте.</param>
        /// <param name="cardNavigationService"><see cref="INavigationService"/>,
        /// совершающий переход на <see cref="CardViewModel"/></param>
        public NavigateCardCommand(CardStore cardStore, INavigationService cardNavigationService)
        {
            this.cardStore = cardStore;
            this.cardNavigationService = cardNavigationService;
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter)
        {
            if (parameter is string cardNumber)
                Task.Run(async () => await FetchCardFromDatabase(cardNumber));

            cardNavigationService?.Navigate();
        }

        /// <summary>
        /// Получает данные о карте из базы данных по номеру карты.
        /// </summary>
        /// <param name="cardNumber">Номер карты для поиска в базе данных.</param>
        /// <returns>Задача <see cref="Task"/>, представляющая асинхронную операцию.</returns>
        private async Task FetchCardFromDatabase(string cardNumber)
        {
            string encryptedCardNumber = EncryptionHelper.Encrypt(cardNumber);
            try
            {
                using (var database = new WalletEntities())
                {
                    var card = await database
                        .Card
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => 
                        c.Number == encryptedCardNumber);

                    if (card != null)
                        cardStore.CurrentCard = card;
                }
            }
            catch (Exception e) 
            { 
                ErrorMessageBox.Show(e); 
            }
            finally
            {
                cardNavigationService?.Navigate();
            }
        }
        #endregion
    }
}
