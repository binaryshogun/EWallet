using EWallet.Components.CS;
using EWallet.NET.Models;
using EWallet.Services;
using EWallet.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда регистрации пользователя.
    /// </summary>
    public sealed class RegisterUserCommand : CommandBase
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

        public bool IsUserRegistered { get; set; } = false;

        #region Methods
        ///<inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object? parameter)
        {
            RegisterUserInDataBase();
        }

        public async void RegisterUserInDataBase()
        {
            using (var dataBase = new WalletEntities())
            {
                try
                {
                    User user = await dataBase
                        .User
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Login == viewModel.Login);

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
                        throw new Exception("Пользователь уже зарегистрирован в системе!");

                    MessageBox.Show("Пользователь успешно зарегистрирован!\n" +
                        "Перенаправление на страницу авторизации...", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    IsUserRegistered = true;
                    authorizationNavigationService?.Navigate();
                }
                catch (Exception ex)
                {
                    ErrorMessageBox.Show(ex);
                }
            }
        }

        /// <inheritdoc cref="AuthorizeUserCommand.GetHash(string, int)"/>
        public static string GetHash(string password, int length)
        {
            using (var hash = SHA1.Create())
            {
                return string
                    .Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password))
                    .Select(x => x.ToString("X2")))
                    .Substring(0, length);
            }
        }
        #endregion
    }
}
