using EWallet.Models;
using EWallet.Stores;
using EWallet.ViewModels;
using System.Data.Entity;

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
            => SavePassportData();

        public async void SavePassportData()
        {
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
            }
        }
    }
}