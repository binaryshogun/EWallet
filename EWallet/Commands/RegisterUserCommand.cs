using EWallet.Components;
using EWallet.Exceptions;
using EWallet.Helpers;
using EWallet.Models;
using EWallet.Services;
using EWallet.Stores;
using EWallet.ViewModels;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда регистрации пользователя.
    /// </summary>
    public sealed class RegisterUserCommand : CommandBase
    {
        #region Fields
        private readonly RegistrationViewModel registrationViewModel;
        private readonly INavigationService accountNavigationService;
        private readonly UserStore userStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RegisterUserCommand"/>.
        /// </summary>
        /// <param name="registrationViewModel"><see cref="RegistrationViewModel"/>,
        /// содержащая данные для регистрации пользователя.</param>
        /// <param name="accountNavigationService"><see cref="INavigationService"/>,
        /// совершающий переход на <see cref="AccountViewModel"/>.</param>
        /// <param name="userStore"><see cref="UserStore"/>,
        /// содержащий информацию о текущем пользователе.</param>
        public RegisterUserCommand(RegistrationViewModel registrationViewModel, 
            INavigationService accountNavigationService, 
            UserStore userStore)
        {
            this.registrationViewModel = registrationViewModel;
            this.accountNavigationService = accountNavigationService;
            this.userStore = userStore;
        }
        #endregion

        #region Methods
        ///<inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter) 
            => Task.Run(RegisterUserInDatabase);

        /// <summary>
        /// Регистрирует пользователя в базе данных.
        /// </summary>
        /// <returns>Задача <see cref="Task"/>, представляющая асинхронную операцию.</returns>
        public async Task RegisterUserInDatabase()
        {
            registrationViewModel.IsUserAuthorizing = true;

            try
            {
                using (var database = new WalletEntities())
                {
                    var user = await FetchUser(database);

                    if (user == null)
                    {
                        int length = 16;
                        if (registrationViewModel.Password.Length < 16)
                            length = registrationViewModel.Password.Length;

                        await AddUserToDatabase(database, length);
                        await AddPassportToDatabase(database);
                    }
                    else
                        throw new UserAlreadyRegistredException("Пользователь уже зарегистрирован в системе!");

                    var registredUser = await FetchUser(database);
                    userStore.CurrentUser = registredUser;
                    accountNavigationService?.Navigate();
                }
            }
            catch (Exception ex) 
            { 
                ErrorMessageBox.Show(ex); 
            }
            finally 
            { 
                registrationViewModel.IsUserAuthorizing = false; 
            }
        }

        /// <summary>
        /// Получает пользователя по логину из базы данных.
        /// </summary>
        /// <param name="database">Экземпляр базы данных <see cref="WalletEntities"/>.</param>
        /// <returns>Экземпляр класса <see cref="User"/> при наличии в базе данных 
        /// или значение <see langword="null"/> при отсутствии в базе данных.</returns>
        private async Task<User> FetchUser(WalletEntities database)
            => await database.User.AsNoTracking().FirstOrDefaultAsync(
                u => u.Login == registrationViewModel.Login);
        /// <summary>
        /// Добавляет пользователя в базу данных.
        /// </summary>
        /// <param name="database">Экземпляр базы данных <see cref="WalletEntities"/>.</param>
        /// <param name="length">Длина хэширования пароля для метода <see cref="HashHelper.GetHash(string, int)"/></param>
        /// <returns>Задача <see cref="Task"/>, представляющая асинхронную операцию.</returns>
        private async Task AddUserToDatabase(WalletEntities database, int length)
        {
            var user = new User()
            {
                Login = registrationViewModel.Login,
                Password = HashHelper.GetHash(registrationViewModel.Password, length),
                RoleID = 1,
                Balance = 0
            };
            database.User.Add(user);
            await database.SaveChangesAsync();
        }
        /// <summary>
        /// Добавляет связанный с пользователем паспорт в базу данных.
        /// </summary>
        /// <param name="database">Экземпляр базы данных <see cref="WalletEntities"/>.</param>
        /// <returns>Задача <see cref="Task"/>, представляющая асинхронную операцию.</returns>
        private async Task AddPassportToDatabase(WalletEntities database)
        {
            var user = await FetchUser(database);
            Passport userPassport = new Passport()
            {
                FirstName = registrationViewModel.FirstName,
                LastName = registrationViewModel.LastName,
                UserID = user.ID
            };

            if (!string.IsNullOrEmpty(registrationViewModel.Patronymic))
                userPassport.Patronymic = registrationViewModel.Patronymic;

            database.Passport.AddOrUpdate(userPassport);
            await database.SaveChangesAsync();
        }
        #endregion
    }
}
