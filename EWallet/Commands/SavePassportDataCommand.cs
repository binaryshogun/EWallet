using EWallet.Components;
using EWallet.Models;
using EWallet.Stores;
using EWallet.ViewModels;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    /// <summary>
    /// Команда сохранения паспортных данных.
    /// </summary>
    public sealed class SavePassportDataCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly UserProfileViewModel userProfileViewModel;
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SavePassportDataCommand"/>.
        /// </summary>
        /// <param name="userStore"><see cref="UserStore"/>,
        /// содержащий данные о текущем пользователе.</param>
        /// <param name="userProfileViewModel"><see cref="UserProfileViewModel"/>,
        /// содержащая паспортные данные для сохранения.</param>
        public SavePassportDataCommand(UserStore userStore, UserProfileViewModel userProfileViewModel)
        {
            this.userStore = userStore;
            this.userProfileViewModel = userProfileViewModel;
        }
        #endregion

        #region Methods
        /// <inheritdoc cref="CommandBase.Execute(object)"/>
        public override void Execute(object parameter) 
            => Task.Run(SavePassportDataInDatabase);

        /// <summary>
        /// Сохраняет паспортные данные в базе данных.
        /// </summary>
        /// <returns>Задача <see cref="Task"/>, представляющая асинхронную операцию.</returns>
        public async Task SavePassportDataInDatabase()
        {
            userProfileViewModel.IsDataSaved = false;
            userProfileViewModel.IsDataSave = true;
            userProfileViewModel.SaveDataMessage = "Данные сохраняются...";

            try
            {
                using (var dataBase = new WalletEntities())
                {
                    var passport = await FetchPassport(dataBase);

                    if (passport != null)
                        EditPassport(dataBase, passport);
                    else
                        CreatePassport(dataBase);

                    await dataBase.SaveChangesAsync();
                }
                userProfileViewModel.SaveDataMessage = "Данные сохранены!";
            }
            catch (Exception e) 
            { 
                ErrorMessageBox.Show(e); 
            }
            finally
            {
                userProfileViewModel.IsDataSave = false;
                userProfileViewModel.SaveDataMessage = "";
                userProfileViewModel.IsDataSaved = true;
            }
        }
        /// <summary>
        /// Получает экземпляр <see cref="Passport"/> для конкретного пользователя.
        /// </summary>
        /// <param name="dataBase">Экземпляр базы данных <see cref="WalletEntities"/>.</param>
        /// <returns>Конкретный экземпляр <see cref="Passport"/> пользователя <see cref="User"/> при наличии в базе данных 
        /// или значение <see langword="null"/> при отсутствии в базе данных.</returns>
        private async Task<Passport> FetchPassport(WalletEntities dataBase)
            => await dataBase.Passport.AsNoTracking().FirstOrDefaultAsync(
                p => p.UserID == userStore.CurrentUser.ID);
        /// <summary>
        /// Обновляет паспортные данные конкретного экземпляра <see cref="Passport"/>.
        /// </summary>
        /// <param name="dataBase">Экземпляр базы данных <see cref="WalletEntities"/>.</param>
        /// <param name="passport">Экземпляр <see cref="Passport"/>, принадлежащий пользователю.</param>
        private void EditPassport(WalletEntities dataBase, Passport passport)
        {
            passport.FirstName = userProfileViewModel.FirstName;
            passport.LastName = userProfileViewModel.LastName;
            passport.Patronymic = userProfileViewModel.DoesUserHavePatronymic
                ? userProfileViewModel.Patronymic : null;
            passport.Number = userProfileViewModel.Number;
            passport.SerialNumber = userProfileViewModel.SerialNumber;
            passport.DivisionCode = userProfileViewModel.DivisionCode;

            dataBase.Passport.AddOrUpdate(passport);
        }
        /// <summary>
        /// Создает новый экземпляр <see cref="Passport"/> из данных <see cref="UserProfileViewModel"/>.
        /// </summary>
        /// <param name="dataBase">Экземпляр базы данных <see cref="WalletEntities"/>.</param>
        private void CreatePassport(WalletEntities dataBase)
        {
            var passport = new Passport()
            {
                FirstName = userProfileViewModel.FirstName,
                LastName = userProfileViewModel.LastName,
                Patronymic = userProfileViewModel.DoesUserHavePatronymic
                                        ? userProfileViewModel.Patronymic : null,
                Number = userProfileViewModel.Number,
                SerialNumber = userProfileViewModel.SerialNumber,
                DivisionCode = userProfileViewModel.DivisionCode,
                UserID = userStore.CurrentUser.ID
            };

            dataBase.Passport.Add(passport);
        }
        #endregion
    }
}