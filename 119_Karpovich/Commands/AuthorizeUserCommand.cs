using _119_Karpovich.Models;
using _119_Karpovich.Stores;
using _119_Karpovich.ViewModels;
using _119_Karpovich.Services;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace _119_Karpovich.Commands
{
    /// <summary>
    /// Команда авторизации пользователя.
    /// </summary>
    internal class AuthorizeUserCommand : CommandBase
    {
        #region Fields
        private readonly AuthorizationViewModel viewModel;
        private readonly INavigationService navigationService;
        private readonly UserStore userStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует команду авторизации.
        /// </summary>
        /// <param name="viewModel">ViewModel данных авторизации.</param>
        /// <param name="navigationStore">Хранилище данных.</param>
        public AuthorizeUserCommand(AuthorizationViewModel viewModel, INavigationService navigationService, UserStore userStore)
        {
            this.viewModel = viewModel;
            this.navigationService = navigationService;
            this.userStore = userStore;
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter)
        {
            using (var dataBase = new WalletEntities())
            {
                int length = 16;
                if (viewModel.Password.Length < 16)
                    length = viewModel.Password.Length;

                string tempPassword = GetHash(viewModel.Password, length);

                User user = dataBase.User
                    .AsNoTracking()
                    .FirstOrDefault(
                    u => u.Login == viewModel.Login
                    && (u.Password == tempPassword || u.Password == "default"));

                if (user != null)
                {
                    userStore.CurrentUser = user;
                    navigationService.Navigate();
                }
                else
                    MessageBox.Show("Неверно введён логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
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
