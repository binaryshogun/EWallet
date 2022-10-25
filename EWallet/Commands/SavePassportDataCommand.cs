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
    public sealed class SavePassportDataCommand : CommandBase
    {
        #region Fields
        private readonly UserStore userStore;
        private readonly UserProfileViewModel userProfileViewModel;
        #endregion

        #region Constructors
        public SavePassportDataCommand(UserStore userStore, UserProfileViewModel userProfileViewModel)
        {
            this.userStore = userStore;
            this.userProfileViewModel = userProfileViewModel;
        }
        #endregion

        #region Methods
        public override void Execute(object parameter) 
            => Task.Run(SavePassportData);

        public async Task SavePassportData()
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
            catch (Exception e) { ErrorMessageBox.Show(e); }
            finally
            {
                userProfileViewModel.IsDataSave = false;
                userProfileViewModel.SaveDataMessage = "";
                userProfileViewModel.IsDataSaved = true;
            }
        }
        private async Task<Passport> FetchPassport(WalletEntities dataBase)
            => await dataBase.Passport.AsNoTracking().FirstOrDefaultAsync(
                p => p.UserID == userStore.CurrentUser.ID);

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