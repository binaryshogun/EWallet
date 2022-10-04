using EWallet.Models;
using EWallet.Stores;
using EWallet.ViewModels;
using EWallet.Services;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using EWallet.Components;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда авторизации пользователя.
    /// </summary>
    public sealed class AuthorizeUserCommand : CommandBase
    {
        #region Fields
        private readonly AuthorizationViewModel viewModel;
        private readonly INavigationService navigationService;
        private readonly UserStore userStore;
        private readonly NavigationBarViewModel navigationBarViewModel;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует команду авторизации.
        /// </summary>
        /// <param name="viewModel">ViewModel данных авторизации.</param>
        /// <param name="navigationStore">Хранилище данных.</param>
        public AuthorizeUserCommand(AuthorizationViewModel viewModel, 
            INavigationService navigationService, 
            UserStore userStore)
        {
            this.viewModel = viewModel;
            this.navigationService = navigationService;
            this.userStore = userStore;
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter) 
            => Task.Run(FetchUserFromDataBase);

        public async void FetchUserFromDataBase()
        {
            using (var dataBase = new WalletEntities())
            {
                int length = 16;
                if (viewModel.Password.Length < 16)
                    length = viewModel.Password.Length;

                string tempPassword = GetHash(viewModel.Password, length);

                try
                {
                    User user = await dataBase
                    .User
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                    u => u.Login == viewModel.Login
                    && u.Password == tempPassword);

                    userStore.CurrentUser = user ?? throw new Exception("Пользователь не найден!");
                    navigationService?.Navigate();

                }
                catch (Exception e) { ErrorMessageBox.Show(e); }
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
        #endregion
    }
}
