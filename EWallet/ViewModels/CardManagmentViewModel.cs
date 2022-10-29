using EWallet.Commands;
using EWallet.Components;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace EWallet.ViewModels
{
    /// <summary>
    /// ViewModel страницы управления картами.
    /// </summary>
    public sealed class CardManagmentViewModel : ViewModelBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly WalletEntities database;

        private ListCollectionView cards;
        private bool isThereCards;
        private bool areCardsLoading;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CardManagmentViewModel"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>,
        /// содержащий данные о текущем пользователе.</param>
        /// <param name="cardStore"><see cref="CardStore"/>,
        /// содержащий данные о карте пользователя.</param>
        /// <param name="accountNavigatonService"><see cref="INavigationService"/>,
        /// совершающий переход на <see cref="AccountViewModel"/>.</param>
        /// <param name="cardNavigationService"><see cref="INavigationService"/>,
        /// совершающий переход на <see cref="CardViewModel"/>.</param>
        public CardManagmentViewModel(UserStore userStore,
            CardStore cardStore,
            INavigationService accountNavigatonService, 
            INavigationService cardNavigationService)
        {
            this.userStore = userStore;

            try
            {
                database = new WalletEntities();
            }
            catch (Exception e) 
            { 
                ErrorMessageBox.Show(e);
                accountNavigatonService?.Navigate();
            }

            FetchCards();

            NavigateAccountCommand = new NavigateCommand(accountNavigatonService);
            NavigateCardCommand = new NavigateCardCommand(cardStore, cardNavigationService);
            DeleteCardCommand = new DeleteCardCommand(cardStore, this);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Список карт, привязанных к пользователю.
        /// </summary>
        public ListCollectionView Cards
        {
            get => cards;
            set
            {
                cards = value;
                OnPropertyChanged(nameof(Cards));
            }
        }
        /// <summary>
        /// Указывает, есть ли у пользователя привязанные карты.
        /// </summary>
        public bool IsThereCards
        {
            get => isThereCards;
            set
            {
                isThereCards = value;
                OnPropertyChanged(nameof(IsThereCards));
            }
        }
        /// <summary>
        /// Указывает, загружается ли список привязанных карт.
        /// </summary>
        public bool AreCardsLoading
        {
            get => areCardsLoading;
            set
            {
                areCardsLoading = value;
                OnPropertyChanged(nameof(AreCardsLoading));
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Получает и задает список привязанных карт <see cref="Cards"/>.
        /// </summary>
        public void FetchCards()
        {
            AreCardsLoading = true;
            try
            {
                var cardsList = database
                    .Card
                    .Where(c => c.UserID == userStore.CurrentUser.ID)
                    .ToList()
                    .Select(c =>
                        new
                        {
                            Number = EncryptionHelper.Decrypt(c.Number),
                            ValidThru = c.ValidThru.ToString("D")
                        })
                    .ToList();
                Cards = new ListCollectionView(cardsList);
                Cards.Refresh();
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally 
            {
                CheckCardsCount();
                AreCardsLoading = false; 
            }
        }
        /// <summary>
        /// Проверяет количество привязанных карт.
        /// </summary>
        private void CheckCardsCount()
        {
            try
            {
                if (Cards.Count > 0)
                    IsThereCards = true;
                else
                    IsThereCards = false;
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
        }
        /// <summary>
        /// Освобождает ресурсы <see cref="CardManagmentViewModel"/>.
        /// </summary>
        public override void Dispose()
        {
            database.Dispose();

            base.Dispose();
        }
        #endregion

        #region Commands
        /// <summary>
        /// Команда перехода на <see cref="AccountViewModel"/>.
        /// </summary>
        public ICommand NavigateAccountCommand { get; }
        /// <summary>
        /// Команда перехода на <see cref="CardViewModel"/>.
        /// </summary>
        public ICommand NavigateCardCommand { get; }
        /// <summary>
        /// Команда для удаления карты из списка привязанных карт.
        /// </summary>
        public ICommand DeleteCardCommand { get; }
        #endregion
    }
}
