using EWallet.Models;
using EWallet.Stores;
using EWallet.ViewModels;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading;
using System.Threading.Tasks;

namespace EWallet.Commands
{
    public sealed class SavePassportDataCommand : CommandBase
    {
        private readonly UserProfileViewModel viewModel;
        private readonly UserStore userStore;

        public SavePassportDataCommand(UserProfileViewModel viewModel, UserStore userStore)
        {
            this.viewModel = viewModel;
            this.userStore = userStore;
        }

        public override void Execute(object parameter) 
            => Task.Run(SavePassportData);

        public async Task SavePassportData()
        {
            viewModel.IsDataSaved = false;
            viewModel.IsDataSave = true;
            viewModel.SaveDataMessage = "Данные сохраняются...";
            using (var dataBase = new WalletEntities())
            {
                Passport passport = await dataBase
                    .Passport
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.UserID == userStore.CurrentUser.ID);

                if (passport != null)
                {
                    passport.FirstName = viewModel.FirstName;
                    passport.LastName = viewModel.LastName;
                    passport.Patronymic = viewModel.DoesUserHavePatronymic 
                        ? viewModel.Patronymic : null;
                    passport.Number = viewModel.Number;
                    passport.SerialNumber = viewModel.SerialNumber;
                    passport.DivisionCode = viewModel.DivisionCode;
                    dataBase.Passport.AddOrUpdate(passport);
                }
                else
                {
                    passport = new Passport()
                    {
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        Patronymic = viewModel.DoesUserHavePatronymic 
                            ? viewModel.Patronymic : null,
                        Number = viewModel.Number,
                        SerialNumber = viewModel.SerialNumber,
                        DivisionCode = viewModel.DivisionCode,
                        UserID = userStore.CurrentUser.ID
                    };
                    dataBase.Passport.Add(passport);
                }

                await dataBase.SaveChangesAsync();

                viewModel.IsDataSave = false;
                viewModel.SaveDataMessage = "Данные сохранены!";
                viewModel.SaveDataMessage = "";
                viewModel.IsDataSaved = true;
            }
        }
    }
}