using _119_Karpovich.Models;
using _119_Karpovich.Services;
using _119_Karpovich.ViewModels;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace _119_Karpovich.Commands
{
    /// <summary>
    /// Команда регистрации пользователя.
    /// </summary>
    internal class RegisterUserCommand : CommandBase
    {
        #region Fields
        private readonly RegistrationViewModel viewModel;
        private readonly INavigationService authorizationNavigationService;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует команду регистрации пользователя.
        /// </summary>
        /// <param name="viewModel">ViewModel страницы регистрации пользователя.</param>
        /// <param name="authorizationNavigationService">Сервис навигации, привязанный к AuthorizationViewModel.</param>
        public RegisterUserCommand(RegistrationViewModel viewModel, INavigationService authorizationNavigationService)
        {
            this.viewModel = viewModel;
            this.authorizationNavigationService = authorizationNavigationService;
        }
        #endregion

        #region Methods
        ///<inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter)
        {
            using (var dataBase = new WalletEntities())
            {
                try
                {
                    User user = dataBase.User.AsNoTracking().FirstOrDefault(u => u.Login == viewModel.Login);

                    if (user == null)
                    {
                        int length = 16;
                        if (viewModel.Password.Length < 16)
                            length = viewModel.Password.Length;

                        user = new User()
                        {
                            Login = viewModel.Login,
                            Password = GetHash(viewModel.Password, length),
                            RoleID = 1,
                            Balance = 0
                        };

                        dataBase.User.Add(user);
                        dataBase.SaveChanges();
                    }
                    else
                        MessageBox.Show("Пользователь уже зарегистрирован в системе!", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                MessageBox.Show("Пользователь успешно зарегистрирован!\n" +
                        "Перенаправление на страницу авторизации...", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            authorizationNavigationService.Navigate();
        }

        /// <inheritdoc cref="AuthorizeUserCommand.GetHash(string, int)"/>
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
