using EWallet.Components.CS;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using EWallet.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
        private readonly INavigationService accountNavigationService;
        private readonly UserStore userStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует команду регистрации пользователя.
        /// </summary>
        /// <param name="viewModel">ViewModel страницы регистрации пользователя.</param>
        /// <param name="accountNavigationService">Сервис навигации, привязанный к AccountViewModel.</param>
        public RegisterUserCommand(RegistrationViewModel viewModel, INavigationService accountNavigationService, UserStore userStore)
        {
            this.viewModel = viewModel;
            this.accountNavigationService = accountNavigationService;
            this.userStore = userStore;
        }
        #endregion

        #region Methods
        ///<inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter) 
            => Task.Run(RegisterUserInDataBase);

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

                        Passport userPassport = new Passport()
                        {
                            FirstName = viewModel.FirstName,
                            LastName = viewModel.LastName,
                            Patronymic = viewModel?.Patronymic,
                            SerialNumber = 0,
                            DivisionCode = 0,
                            Number = 0,
                            UserID = dataBase
                                    .User
                                    .AsNoTracking()
                                    .FirstOrDefault(u => u.Login == viewModel.Login).ID
                        };
                    }
                    else
                        throw new Exception("Пользователь уже зарегистрирован в системе!");

                    MessageBox.Show("Пользователь успешно зарегистрирован!\n" +
                        "Перенаправление на страницу авторизации...", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    userStore.CurrentUser = user;
                    accountNavigationService?.Navigate();
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
