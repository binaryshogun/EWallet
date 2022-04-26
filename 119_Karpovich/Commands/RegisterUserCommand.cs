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
    public class RegisterUserCommand : CommandBase
    {
        private readonly RegistrationViewModel viewModel;
        private readonly NavigationService<AuthorizationViewModel> navigationService;
        private readonly WalletEntities dataBase;

        public RegisterUserCommand(RegistrationViewModel viewModel, NavigationService<AuthorizationViewModel> navigationService)
        {
            this.viewModel = viewModel;
            this.navigationService = navigationService;
            dataBase = new WalletEntities();
        }

        public override void Execute(object parameter)
        {
            using (dataBase)
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

            navigationService.Navigate();
        }

        public static string GetHash(string password, int length)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2"))).Substring(0, length);
            }
        }
    }
}
