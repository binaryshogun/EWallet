using EWallet.Models;
using EWallet.Stores;
using EWallet.ViewModels;
using EWallet.Services;
using EWallet.Components;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using EWallet.Helpers;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда авторизации пользователя.
    /// </summary>
    public sealed class AuthorizeUserCommand : CommandBase
    {
        #region Fields
        private readonly AuthorizationViewModel viewModel;
        private readonly INavigationService accountNavigationService;
        private readonly UserStore userStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthorizeUserCommand"/>.
        /// </summary>
        /// <param name="viewModel"><see cref="AuthorizationViewModel"/>, содержащая данные для входа.</param>
        /// <param name="navigationStore">Хранилище данных.</param>
        public AuthorizeUserCommand(AuthorizationViewModel viewModel, 
            INavigationService accountNavigationService, 
            UserStore userStore)
        {
            this.viewModel = viewModel;
            this.accountNavigationService = accountNavigationService;
            this.userStore = userStore;
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter) 
            => Task.Run(FetchUserFromDatabase);

        public async Task FetchUserFromDatabase()
        {
            viewModel.IsUserAuthorizing = true;

            try
            {
                using (var database = new WalletEntities())
                {
                    int length = 16;
                    if (viewModel.Password.Length < 16)
                        length = viewModel.Password.Length;

                    string tempPassword = HashHelper.GetHash(viewModel.Password, length);
                    var user = await FetchUser(database, tempPassword);
                    userStore.CurrentUser = user ?? throw new Exception("Пользователь не найден!");
                }

                accountNavigationService?.Navigate();
            }
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally
            {
                viewModel.IsUserAuthorizing = false;
            }
        }
        private async Task<User> FetchUser(WalletEntities database, string tempPassword) 
            => await database.User.AsNoTracking().FirstOrDefaultAsync(
                u => u.Login == viewModel.Login && u.Password == tempPassword);
        #endregion
    }
}
