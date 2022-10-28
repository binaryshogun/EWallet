using EWallet.Models;
using EWallet.Stores;
using EWallet.ViewModels;
using EWallet.Services;
using EWallet.Components;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using EWallet.Helpers;
using EWallet.Exceptions;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда авторизации пользователя.
    /// </summary>
    public sealed class AuthorizeUserCommand : CommandBase
    {
        #region Fields
        private readonly AuthorizationViewModel authorizationViewModel;
        private readonly INavigationService accountNavigationService;
        private readonly UserStore userStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthorizeUserCommand"/>.
        /// </summary>
        /// <param name="authorizationViewModel"><see cref="AuthorizationViewModel"/>, 
        /// содержащая данные для входа.</param>
        /// <param name="accountNavigationService"><see cref="INavigationService"/>, 
        /// совершающий переход на <see cref="AccountViewModel"/>.</param>
        /// <param name="userStore"><see cref="UserStore"/>, содержащий данные о текущем пользователе.</param>
        public AuthorizeUserCommand(AuthorizationViewModel authorizationViewModel, 
            INavigationService accountNavigationService, 
            UserStore userStore)
        {
            this.authorizationViewModel = authorizationViewModel;
            this.accountNavigationService = accountNavigationService;
            this.userStore = userStore;
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter) 
            => Task.Run(FetchUserFromDatabase);

        /// <summary>
        /// Получает пользователя из базы данных и проводит попытку авторизации.
        /// </summary>
        /// <exception cref="UserNotFoundException">
        /// Возникает, когда пользователь не проходит авторизацию.</exception>
        /// <returns>Задача <see cref="Task"/>, представляющая асинхронную операцию.</returns>
        private async Task FetchUserFromDatabase()
        {
            authorizationViewModel.IsUserAuthorizing = true;

            try
            {
                using (var database = new WalletEntities())
                {
                    int length = 16;
                    if (authorizationViewModel.Password.Length < 16)
                        length = authorizationViewModel.Password.Length;

                    string tempPassword = HashHelper.GetHash(authorizationViewModel.Password, length);
                    var user = await FetchUser(database, tempPassword);
                    userStore.CurrentUser = user ?? throw new UserNotFoundException("Пользователь не найден!");
                }

                accountNavigationService?.Navigate();
            }
            catch (Exception e) 
            { 
                ErrorMessageBox.Show(e); 
            }
            finally
            {
                authorizationViewModel.IsUserAuthorizing = false;
            }
        }
        /// <summary>
        /// Проводит попытку получения пользователя из базы данных по логину и паролю.
        /// </summary>
        /// <param name="database">Экземпляр базы данных <see cref="WalletEntities"/>.</param>
        /// <param name="tempPassword">Хэшированный с помощью 
        /// <see cref="HashHelper"/> пароль.</param>
        /// <returns>Объект <see cref="User"/> при наличии в базе данных или значение <see langword="null"/> при отсутствии в базе данных.</returns>
        private async Task<User> FetchUser(WalletEntities database, string tempPassword) 
            => await database.User.AsNoTracking().FirstOrDefaultAsync(
                u => u.Login == authorizationViewModel.Login && u.Password == tempPassword);
        #endregion
    }
}
